@@setlocal
@@set POWERSHELL_BAT_ARGS=%*
@@if defined POWERSHELL_BAT_ARGS set POWERSHELL_BAT_ARGS=%POWERSHELL_BAT_ARGS:"=\"%
@@powershell.exe -Command Invoke-Expression $('$args=@(^&{$args} %POWERSHELL_BAT_ARGS%);'+[String]::Join([char]10, $((Get-Content '%~f0') -notmatch '^^@@'))) & goto :EOF


$ProjectConsole = 1;
$ProjectLibrary = 2;
$ProjectUnitTest = 3;

Function Main($rootPath, $name)
{
    $fullPath = "$rootPath\$name";
    $exists = Test-Path $fullPath;
    if (!$exists)
    {
        New-Item $fullPath -Type Directory | Out-Null;
    }

    $projects = CreateProjects $name;
    WriteProjects $name $fullPath $projects;
    WriteStyleCopSettings "$fullPath\StyleCop.settings";
    WriteSolutionFile "$fullPath\$name.sln" $projects;
}

Function CreateProjects($name)
{
    $app = CreateProject "$name.App" "source" $ProjectConsole "Program.cs";
    $core = CreateProject "$name.Core" "source" $ProjectLibrary "Class1.cs";
    $test = CreateProject "$name.Test.Unit" "test" $ProjectUnitTest "Test1.cs";

    $app.ProjectReferences += $core;
    $test.ProjectReferences += $core;

    return ($app, $core, $test);
}

Function CreateProject($name, $folderName, $type, $fileToInclude)
{
    $path = "$folderName\$name\$name.csproj";
    $properties = 
    @{
        "Name" = $name;
        "Path" = $path;
        "Id" = [guid]::NewGuid().ToString("B").ToUpperInvariant();
        "ProjectType" = $type;
        "ProjectReferences" = @();
        "FileToInclude" = $fileToInclude;
    };

    return New-Object PSObject -Property $properties;
}

Function WriteProjects($solutionName, $rootPath, [psobject[]]$projects)
{
    foreach ($project in $projects)
    {
        $file = New-Object -Type "System.IO.FileInfo" -ArgumentList ("$rootPath\" + $project.Path);
        Write-Host "Writing project file '$file'...";
        $parentDir = $file.Directory;
        if (!$parentDir.Exists)
        {
            $parentDir.Create();
        }

        GetProjectText $solutionName $project | Out-File $file.FullName -Encoding "UTF8";
    }
}

Function GetProjectText($solutionName, $project)
{
    $id = $project.Id;
    $name = $project.Name;
    $afterHeaderText = "";
    $fileToInclude = $project.FileToInclude;
    $filesText = "";
    if ($project.ProjectType -eq $ProjectConsole)
    {
        $outputType = "Exe";
        $rootNamespace = $solutionName;
        $afterHeaderText +=
@'
    <PlatformTarget>AnyCPU</PlatformTarget>
    <Prefer32Bit>false</Prefer32Bit>

'@;
    }
    elseif ($project.ProjectType -eq $ProjectLibrary)
    {
        $outputType = "Library";
        $rootNamespace = $solutionName;
    }
    else
    {
        $outputType = "Library";
        $rootNamespace = "$solutionName.Test.Unit";
    }

    $headerText =
@'
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="12.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')" />
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
'@;
    $headerText +=
@"
    <ProjectGuid>$id</ProjectGuid>
    <OutputType>$outputType</OutputType>
    <RootNamespace>$rootNamespace</RootNamespace>
    <AssemblyName>$name</AssemblyName>

"@;

    $headerText +=
@'
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <OutputPath>..\..\bin\$(Configuration)\</OutputPath>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <StyleCopTreatErrorsAsWarnings>false</StyleCopTreatErrorsAsWarnings>

'@;

    $afterHeaderText +=
@'
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
  </PropertyGroup>
  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <DefineConstants>TRACE</DefineConstants>
  </PropertyGroup>
  <ItemGroup>
    <Reference Include="System" />
    <Reference Include="System.Core" />
  </ItemGroup>
  <ItemGroup>
    <Compile Include="Properties\AssemblyInfo.cs" />

'@;

    $filesText =
@"
    <Compile Include="$fileToInclude" />
  </ItemGroup>

"@;

    $projectReferencesText = "";
    if ($project.ProjectReferences.Length -gt 0)
    {
        $projectReferencesText += @'
  <ItemGroup>

'@;
        foreach ($refProject in $project.ProjectReferences)
        {
            $includePath = GetIncludePath $project.Path $refProject.Path;
            $refName = $refProject.Name;
            $refId = $refProject.Id;
            $projectReferencesText +=
@"
    <ProjectReference Include="$includePath">
      <Project>$refId</Project>
      <Name>$refName</Name>
    </ProjectReference>

"@;
        }

        $projectReferencesText += @'
  </ItemGroup>

'@;
    }

    $footerText =
@'
  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  <Import Project="$(MSBuildExtensionsPath)\StyleCop\v4.7\StyleCop.Targets" />
</Project>
'@;

    $text = $headerText + $afterHeaderText + $filesText + $projectReferencesText + $footerText;
    $text = $text -Replace "`n", "`r`n";
    return $text;
}

Function GetIncludePath($source, $dest)
{
    $tree = @{};
    $current = $tree;
    $depth = 0;
    foreach ($part in $source.Split('\'))
    {
        $current[$part] = @{};
        $current = $current[$part];
        ++$depth;
    }

    $current = $tree;
    $includePathParts = @();
    $stillMatches = $true;
    foreach ($part in $dest.Split('\'))
    {
        if ($stillMatches)
        {
            if ($current.ContainsKey($part))
            {
                --$depth;
                $current = $current[$part];
            }
            else
            {
                $stillMatches = $false;
                for ($i = 1; $i -lt $depth; ++$i)
                {
                    $includePathParts += "..";
                }
            }
        }

        if (!$stillMatches)
        {
            $includePathParts += $part;
        }
    }

    return [string]::Join('\', $includePathParts);
}

Function WriteSolutionFile($fullPath, [psobject[]]$projects)
{
    Write-Host "Writing solution file '$fullPath'...";
    $headerText =
@"

Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Express 2013 for Windows Desktop
VisualStudioVersion = 12.0.21005.1
MinimumVisualStudioVersion = 10.0.40219.1

"@;
    $projectsText = "";
    $globalSectionText =
@"
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution

"@;
    $footerText =
@"
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
"@;

    foreach ($project in $projects)
    {
        $name = $project.Name;
        $path = $project.Path;
        $id = $project.Id;
        $projectsText +=
@"
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "$name", "$path", "$id"
EndProject

"@;
        $globalSectionText += 
@"
		$id.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		$id.Debug|Any CPU.Build.0 = Debug|Any CPU
		$id.Release|Any CPU.ActiveCfg = Release|Any CPU
		$id.Release|Any CPU.Build.0 = Release|Any CPU

"@;
    }

    $text = $headerText + $projectsText + $globalSectionText + $footerText;
    $text = $text -Replace "`n", "`r`n";
    $text | Out-File $fullPath -Encoding "UTF8";
}

Function WriteStyleCopSettings($fullPath)
{
    Write-Host "Writing StyleCop settings file '$fullPath'...";
    $text =
@"
<StyleCopSettings Version="105">
  <GlobalSettings>
    <BooleanProperty Name="AutoCheckForUpdate">False</BooleanProperty>
  </GlobalSettings>
  <Parsers>
    <Parser ParserId="Microsoft.StyleCop.CSharp.CsParser">
      <ParserSettings>
        <CollectionProperty Name="GeneratedFileFilters">
          <Value>\.g\.cs$</Value>
          <Value>\.generated\.cs$</Value>
          <Value>\.g\.i\.cs$</Value>
          <Value>TemporaryGeneratedFile_.*\.cs$</Value>
        </CollectionProperty>
      </ParserSettings>
    </Parser>
  </Parsers>
  <Analyzers>
    <Analyzer AnalyzerId="StyleCop.CSharp.DocumentationRules">
      <Rules>
        <Rule Name="ElementsMustBeDocumented">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="PartialElementsMustBeDocumented">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="EnumerationItemsMustBeDocumented">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="DocumentationMustContainValidXml">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementDocumentationMustHaveSummary">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="PartialElementDocumentationMustHaveSummary">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementDocumentationMustHaveSummaryText">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="PartialElementDocumentationMustHaveSummaryText">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementDocumentationMustNotHaveDefaultSummary">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementParametersMustBeDocumented">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementParameterDocumentationMustMatchElementParameters">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementParameterDocumentationMustDeclareParameterName">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementParameterDocumentationMustHaveText">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementReturnValueMustBeDocumented">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementReturnValueDocumentationMustHaveText">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="VoidReturnValueMustNotBeDocumented">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="GenericTypeParametersMustBeDocumented">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="GenericTypeParametersMustBeDocumentedPartialClass">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="GenericTypeParameterDocumentationMustMatchTypeParameters">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="GenericTypeParameterDocumentationMustDeclareParameterName">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="GenericTypeParameterDocumentationMustHaveText">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="PropertySummaryDocumentationMustMatchAccessors">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="PropertySummaryDocumentationMustOmitSetAccessorWithRestrictedAccess">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementDocumentationMustNotBeCopiedAndPasted">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="SingleLineCommentsMustNotUseDocumentationStyleSlashes">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="DocumentationTextMustNotBeEmpty">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="DocumentationTextMustContainWhitespace">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="DocumentationMustMeetCharacterPercentage">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ConstructorSummaryDocumentationMustBeginWithStandardText">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="DestructorSummaryDocumentationMustBeginWithStandardText">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="DocumentationHeadersMustNotContainBlankLines">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="IncludedDocumentationXPathDoesNotExist">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="IncludeNodeDoesNotContainValidFileAndPath">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="InheritDocMustBeUsedWithInheritingClass">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
        <Rule Name="ElementDocumentationMustBeSpelledCorrectly">
          <RuleSettings>
            <BooleanProperty Name="Enabled">False</BooleanProperty>
          </RuleSettings>
        </Rule>
      </Rules>
      <AnalyzerSettings />
    </Analyzer>
  </Analyzers>
</StyleCopSettings>
"@;
    $text = $text -Replace "`n", "`r`n";
    $text | Out-File $fullPath -Encoding "UTF8";
}

Function PrintUsage()
{
    Write-Host "InitProjects.cmd <root-path> <project-name>";
    Write-Host "";
    Write-Host "Creates a simple project structure using the provided root path and project name.";
}

if ($args.Length -ne 2)
{
    PrintUsage;
    Exit 1;
}

Main $args[0] $args[1];

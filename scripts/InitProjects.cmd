@@setlocal
@@set POWERSHELL_BAT_ARGS=%*
@@if defined POWERSHELL_BAT_ARGS set POWERSHELL_BAT_ARGS=%POWERSHELL_BAT_ARGS:"=\"%
@@powershell.exe -Command Invoke-Expression $('$args=@(^&{$args} %POWERSHELL_BAT_ARGS%);'+[String]::Join([char]10, $((Get-Content '%~f0') -notmatch '^^@@'))) & goto :EOF

$ProjectConsole = 1;
$ProjectLibrary = 2;
$ProjectUnitTest = 3;

Function Main($rootPath, $name, $company, $xunitPath, $year)
{
    $fullPath = "$rootPath\$name";
    MakeDir $fullPath;

    $projects = CreateProjects $name;
    WriteProjects $name $fullPath $company $year $projects;
    WriteStyleCopSettings "$fullPath\StyleCop.settings";
    WriteSolutionFile "$fullPath\$name.sln" $projects;
    CopyXunit $xunitPath "$fullPath\external\xUnit.net";
}

Function MakeDir($fullPath)
{
    $exists = Test-Path $fullPath;
    if (!$exists)
    {
        New-Item $fullPath -Type Directory | Out-Null;
    }
}

Function CopyXunit($xunitPath, $destPath)
{
    Write-Host "Copying xUnit.net dependencies from '$xunitPath' to '$destPath'...";
    MakeDir $destPath;
    $files =
    (
        "HTML.xslt",
        "license.txt",
        "NUnitXml.xslt",
        "xunit.console.clr4.exe",
        "xunit.console.clr4.exe.config",
        "xunit.dll",
        "xunit.runner.msbuild.dll",
        "xunit.runner.utility.dll"
    );
    
    foreach ($file in $files)
    {
        Copy-Item "$xunitPath\$file" $destPath;
    }
}

Function CreateProjects($name)
{
    $app = CreateProject "$name.App" "source" $ProjectConsole "Program.cs" $name;
    $core = CreateProject "$name.Core" "source" $ProjectLibrary "Class1.cs" $name;
    $test = CreateProject "$name.Test.Unit" "test" $ProjectUnitTest "Test1.cs" "$name.Test.Unit";

    $app.ProjectReferences += $core;
    $test.ProjectReferences += $core;

    return ($app, $core, $test);
}

Function CreateProject($name, $folderName, $type, $fileToInclude, $rootNamespace)
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
        "RootNamespace" = $rootNamespace;
    };

    return New-Object PSObject -Property $properties;
}

Function WriteProjects($solutionName, $rootPath, $company, $year, [psobject[]]$projects)
{
    foreach ($project in $projects)
    {
        $file = New-Object -Type "System.IO.FileInfo" -ArgumentList ("$rootPath\" + $project.Path);
        $projectName = $project.Name;
        Write-Host "Preparing project '$projectName'...";
        $propertiesDir = $file.Directory.FullName + "\Properties";
        MakeDir $propertiesDir;

        $projectText = GetProjectText $solutionName $project;
        WriteAllText $file.FullName $projectText;
        WriteSourceFile $rootPath $company $project;
        WriteAssemblyInfo $propertiesDir "AssemblyInfo.cs" $company $project $year;
    }
}

Function GetHeaderText($sourceFile, $company)
{
    $text =
@"
//-----------------------------------------------------------------------
// <copyright file="$sourceFile" company="$company">
// Copyright (c) $company. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
"@;
    return $text;
}

Function WriteAssemblyInfo($directory, $sourceFile, $company, $project, $year)
{
    $projectName = $project.Name;
    $assemblyGuid = [guid]::NewGuid().ToString("D");
    $headerText = GetHeaderText $sourceFile $company;
    $text +=
@"
$headerText

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("$projectName")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("$projectName")]
[assembly: AssemblyCopyright("Copyright ©  $year")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: Guid("$assemblyGuid")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
"@;

    $fileName = $directory + "\" + $sourceFile;
    WriteAllText $fileName $text;
}

Function WriteSourceFile($rootPath, $company, $project)
{
    $rootNamespace = $project.RootNamespace;
    $sourceFile = $project.FileToInclude;
    $headerText = GetHeaderText $sourceFile $company;
    $text = 
@"
$headerText

namespace $rootNamespace

"@;
    if ($sourceFile -eq "Program.cs")
    {
        $text +=
@"
{
    using System;

    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Class1 c = new Class1("world");
            string name = c.DoAsync().Result;
            Console.WriteLine("Hello, {0}!", name);
        }
    }
}
"@;
    }
    elseif ($project.FileToInclude -eq "Class1.cs")
    {
        $text +=
@"
{
    using System;
    using System.Threading.Tasks;

    public class Class1
    {
        private readonly string name;

        public Class1(string name)
        {
            this.name = name;
        }

        public Task<string> DoAsync()
        {
            return Task.FromResult(this.name);
        }
    }
}
"@;
    }
    elseif ($project.FileToInclude -eq "Test1.cs")
    {
        $text +=
@"
{
    using System.Threading.Tasks;
    using Xunit;

    public class Test1
    {
        public Test1()
        {
        }

        [Fact]
        public void Should_return_name_after_completing_sync()
        {
            Class1 c = new Class1("MyName");
            
            Task<string> task = c.DoAsync();

            Assert.True(task.IsCompleted);
            Assert.Equal("MyName", task.Result);
        }
    }
}
"@;
    }

    $fullPath = $rootPath + "\" + $project.Path;
    $fileInfo = New-Object -TypeName "System.IO.FileInfo" -ArgumentList $fullPath;
    $fileName = $fileInfo.Directory.FullName + "\" + $project.FileToInclude;
    WriteAllText $fileName $text;
}

Function GetProjectText($solutionName, $project)
{
    $addXunit = $false;
    $id = $project.Id;
    $name = $project.Name;
    $afterHeaderText = "";
    $fileToInclude = $project.FileToInclude;
    $filesText = "";
    $rootNamespace = $project.RootNamespace;
    if ($project.ProjectType -eq $ProjectConsole)
    {
        $outputType = "Exe";
        $afterHeaderText +=
@'
    <PlatformTarget>AnyCPU</PlatformTarget>
    <Prefer32Bit>false</Prefer32Bit>

'@;
    }
    elseif ($project.ProjectType -eq $ProjectLibrary)
    {
        $outputType = "Library";
    }
    else
    {
        $outputType = "Library";
        $addXunit = $true;
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

    if ($addXunit)
    {
        $headerText +=
@'
    <XUnitPath Condition=" '$(XUnitPath)' == '' ">..\..\external\xUnit.net</XUnitPath>

'@;
    }

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

'@;

    if ($addXunit)
    {
        $afterHeaderText +=
@'
    <Reference Include="xunit">
      <HintPath>$(XUnitPath)\xunit.dll</HintPath>
    </Reference>

'@;
    }

    $afterHeaderText +=
@'
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

'@;

    if ($addXunit)
    {
        $footerText +=
@'
  <UsingTask AssemblyFile="$(XUnitPath)\xunit.runner.msbuild.dll" TaskName="Xunit.Runner.MSBuild.xunit" />
  <Target Name="AfterBuild">
    <xunit Assembly="$(TargetPath)" />
  </Target>

'@;
    }

    $footerText +=
@'
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
    Write-Host "Preparing solution file...";
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
    WriteAllText $fullPath $text;
}

Function WriteAllText($fullPath, $text)
{
    Write-Host "Writing file '$fullPath'...";
    $text = $text -Replace "`n", "`r`n";
    $text | Out-File $fullPath -Encoding "UTF8";
}

Function WriteStyleCopSettings($fullPath)
{
    Write-Host "Preparing StyleCop settings file...";
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
    WriteAllText $fullPath $text;
}

Function PrintUsage()
{
    Write-Host "InitProjects.cmd <root-path> <project-name> <company-name> <xunit-path>";
    Write-Host "";
    Write-Host "Creates a simple project structure using the provided root path, project name,";
    Write-Host "company name (used for the copyright banner in the files), and path to Xunit.net";
    Write-Host "binary files (for the unit test project).";
}

if ($args.Length -ne 4)
{
    PrintUsage;
    Exit 1;
}

$currentYear = [datetime]::Now.Year;
Main $args[0] $args[1] $args[2] $args[3] $currentYear;

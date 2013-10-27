@@setlocal
@@set POWERSHELL_BAT_ARGS=%*
@@if defined POWERSHELL_BAT_ARGS set POWERSHELL_BAT_ARGS=%POWERSHELL_BAT_ARGS:"=\"%
@@powershell.exe -Command Invoke-Expression $('$args=@(^&{$args} %POWERSHELL_BAT_ARGS%);'+[String]::Join([char]10, $((Get-Content '%~f0') -notmatch '^^@@'))) & goto :EOF

Function Main($rootPath, $name)
{
    $fullPath = "$rootPath\$name";
    $exists = Test-Path $fullPath;
    if (!$exists)
    {
        New-Item $fullPath -Type Directory | Out-Null;
    }

    $projects = CreateProjects $name;
    WriteProjects $fullPath $projects;
    WriteStyleCopSettings "$fullPath\StyleCop.settings";
    WriteSolutionFile "$fullPath\$name.sln" $projects;
}

Function CreateProjects($name)
{
    $app = CreateProject "$name.App" "source" $false;
    $core = CreateProject "$name.Core" "source" $false;
    $test = CreateProject "$name.Test.Unit" "test" $true;

    $app.ProjectReferences += $core;
    $test.ProjectReferences += $core;

    return ($app, $core, $test);
}

Function CreateProject($name, $folderName, $isUnitTest)
{
    $path = "$folderName\$name\$name.csproj";
    $properties = 
    @{
        "Name" = $name;
        "Path" = $path;
        "Id" = [guid]::NewGuid();
        "IsUnitTest" = $isUnitTest;
        "ProjectReferences" = @();
    };

    return New-Object PSObject -Property $properties;
}

Function WriteProjects($rootPath, [psobject[]]$projects)
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

        "-- TODO --" | Out-File $file.FullName -Encoding "UTF8";
    }
}

Function WriteSolutionFile($fullPath, [psobject[]]$projects)
{
    Write-Host "Writing solution file '$fullPath'...";
    $headerText = @"

Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Express 2013 for Windows Desktop
VisualStudioVersion = 12.0.21005.1
MinimumVisualStudioVersion = 10.0.40219.1

"@;
    $projectsText = "";
    $globalSectionText = @"
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution

"@;
    $footerText = @"
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
        $id = $project.Id.ToString("B").ToUpperInvariant();
        $projectsText += @"
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "$name", "$path", "$id"
EndProject

"@;
        $globalSectionText += @"
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
    $text = @"
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

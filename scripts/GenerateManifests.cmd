@@setlocal
@@set POWERSHELL_BAT_ARGS=%*
@@if defined POWERSHELL_BAT_ARGS set POWERSHELL_BAT_ARGS=%POWERSHELL_BAT_ARGS:"=\"%
@@powershell.exe -Command Invoke-Expression $('$args=@(^&{$args} %POWERSHELL_BAT_ARGS%);'+[String]::Join([char]10, $((Get-Content '%~f0') -notmatch '^^@@'))) & goto :EOF

if ($args.Length -lt 1 -or $args.Length -gt 2)
{
    Write-Host "Specify an assembly path and an optional output directory.";
    Exit 1;
}

$assemblyPath = $args[0];
$outputDirectory = ".";

if ($args.Length -eq 2)
{
    $outputDirectory = $args[1];
}

$eventSourceType = [System.Diagnostics.Tracing.EventSource];
$assembly = [System.Reflection.Assembly]::LoadFrom($args[0]);
$types = $assembly.GetTypes();
foreach ($type in $types)
{
    if ($type.IsSubclassOf($eventSourceType))
    {
        Write-Host "Found type '$type'.";
        $text = [System.Diagnostics.Tracing.EventSource]::GenerateManifest($type, "NOT-USED");
        $fileName = "$outputDirectory\" + $type.FullName.Replace(".", "-") + ".man";
        Write-Host "Writing manifest to '$fileName'...";
        $text | Out-File $fileName;
    }
}

Write-Host "Done.";

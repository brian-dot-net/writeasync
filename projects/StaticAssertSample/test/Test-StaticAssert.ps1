[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$ClExe,
    [Parameter(Mandatory=$true)]
    [string]$Symbol,
    [Parameter(Mandatory=$true)]
    [string]$Pattern,
    [Parameter(Mandatory=$true)]
    [string[]]$CommandFile,
    [Parameter(Mandatory=$true)]
    [string]$TestFile
)

Function Exit-Task($errorText) {
    Write-Host "$($PSCommandPath): error : $errorText"
    Exit 1
}

$clArgs = ''
$outPath = ''
foreach ($file in $CommandFile) {
    if (!(Test-Path $file)) {
        Exit-Task "Command file '$file' not found"
    }

    $lines = Get-Content $file
    # First line has the following format:
    # ^X:\SOME\FULL\PATH\FILE.CPP
    $cppFile = Split-Path ($lines[0].Substring(1)) -Leaf
    if ($cppFile -eq $TestFile) {
        # Second line has actual args to CL.exe
        $clArgs = $lines[1]
        break
    }
}

if (!$clArgs) {
    Exit-Task "Could not find CL args for test file '$TestFile'"
}

$clArgs += " /D$Symbol"
$cmdText = "`"$ClExe`" $clArgs"
Write-Host $cmdText
# The CL.exe path (and possibly some arguments) will be quoted. This causes
# trouble for invoking the expression directly, so invoke via cmd.exe.
$output = & cmd.exe /c $cmdText
if ($LASTEXITCODE -eq 0) {
    Exit-Task "Compilation unexpectedly succeeded for '$Symbol'"
}

$matched = $false
$output | Select-String -Pattern $Pattern | ForEach-Object {
    $matched = $true
    Write-Host "Found matching output line for '$Symbol': $_"
}

if (!$matched) {
    $output | Write-Host
    Exit-Task "Did not find output line for '$Symbol' matching '$Pattern'"
}
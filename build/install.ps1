# Run From Root Of Repository
dotnet publish -c Release

$installPath = Join-Path $env:USERPROFILE ".filesync" "bin" 
$backupPath = Join-Path $env:USERPROFILE ".filesync" "backup" 
$buildPath = Resolve-Path "src\FileSync\bin\Release\net8.0\publish"

if(!(Test-Path $installPath)) {
    New-Item -ItemType Directory -Path $installPath | Out-Null
}

if(!(Test-Path $backupPath)) {
    New-Item -ItemType Directory -Path $backupPath | Out-Null
}

Get-ChildItem $installPath -Recurse | Move-Item -Destination $backupPath
Get-ChildItem $buildPath -Recurse | Copy-Item -Destination $installPath

Get-ChildItem $installPath -Recurse



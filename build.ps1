$cd = Get-Location

# build libs
Get-ChildItem ".\src\Lib" -File -Filter "build.ps1" -Recurse | 
Foreach-Object {
    Set-Location $_.DirectoryName 
    & ./build.ps1 $args
}

Set-Location $cd

if ($LASTEXITCODE -ne 0)
{
    exit $LASTEXITCODE
}
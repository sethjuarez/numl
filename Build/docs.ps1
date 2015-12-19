# Set Path
$sourceDir = resolve-path ..
$workingDir ="$sourceDir\Working" 
$docsDir = "$sourceDir\Docs"
$workingDocs = "$workingDir\Docs"

if (Test-Path -path $workingDocs)
{
    Write-Host "Deleting existing docs directory $workingDocs"
    Remove-Item $workingDocs -Force -Recurse -ErrorAction SilentlyContinue
}

New-Item -Path $workingDocs -ItemType Directory

robocopy $docsDir $workingDocs /MIR /NP /XD obj

Set-Location $workingDocs
docfx --logLevel Verbose

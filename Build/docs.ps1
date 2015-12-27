# Set Path
$sourceDir = resolve-path ..
$workingDir ="$sourceDir\Working" 
$docsDir = "$sourceDir\Docs"
$workingDocs = "$workingDir\Docs"
$webPath = 'C:\numl\numl.web'

if (Test-Path -path $workingDocs)
{
    Write-Host "Deleting existing docs directory $workingDocs"
    Remove-Item $workingDocs -Force -Recurse -ErrorAction SilentlyContinue
}

New-Item -Path $workingDocs -ItemType Directory

robocopy $docsDir $workingDocs /MIR /NP /XD obj

Set-Location $workingDocs
docfx --logLevel Verbose

Write-Host "Copying index.html over..."

cp $docsDir\index.html $workingDocs\_site\index.html

Write-Host "clearning out website"

Remove-Item $webPath\* -recurse -exclude *.git*,*.md

Write-Host "moving over to numl.web"
robocopy $workingDocs\_site  $webPath /E /NP /XF .manifest favicon.ico logo.svg


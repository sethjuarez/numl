properties {
    $baseDir = resolve-path ..
    $workingDir ="$baseDir\Working" 
    $docsDir = "$baseDir\Docs"
    $workingDocs = "$workingDir\Docs"
    $webPath = resolve-path ..\..\numl.web
}


task default -depends Docs

task Docs {
    
    if (Test-Path -path $workingDocs)
    {
        Write-Host "Deleting existing docs directory $workingDocs"
        Remove-Item $workingDocs -Force -Recurse -ErrorAction SilentlyContinue
    }
    
    Write-Host "Creating $workingDocs"
    New-Item -Path $workingDocs -ItemType Directory
    
    robocopy $docsDir $workingDocs /MIR /NP /XD obj | Out-Default

    Set-Location $workingDocs
    
	exec { .\..\..\Tools\docfx\docfx.exe --logLevel Verbose } | Out-Default

    # maybe finalize site here...
    Write-Host "Copying index.html over..."

    exec { cp $docsDir\index.html $workingDocs\_site\index.html } | Out-Default
    
    ## End of standard build
    
    Write-Host "clearning out website"

    Remove-Item $webPath\* -recurse -exclude *.git*,*.md

    Write-Host "moving over to numl.web"
    robocopy $workingDocs\_site  $webPath /E /NP /XF .manifest favicon.ico logo.svg
}

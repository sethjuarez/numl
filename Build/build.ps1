properties {
	$majorVersion = "0.9"
	$majorWithReleaseVersion = "0.9.6"
	$nugetPrelease = "beta"
	$packageId = "numl"
	$version = GetVersion $majorWithReleaseVersion
	$treatWarningsAsErrors = $true
	$workingName = if ($workingName) {$workingName} else {"Working"}
	$baseDir  = resolve-path ..
	$buildDir = "$baseDir\Build"
	$sourceDir = "$baseDir\Src"
	$toolsDir = "$baseDir\Tools"
	$docDir = "$baseDir\Docs"
	$workingDir = "$baseDir\$workingName"
	$workingSourceDir = "$workingDir\Src"
	$dnvmVersion = "1.0.0-rc1-update1"
}

framework '4.6x86'
include .\helpers.ps1

task default -depends Docs

task Clean {
	Write-Host "Setting location to $baseDir"
	Set-Location $baseDir
	
	Write-Host "Clearing all project artifacts."
	
	# Define files and directories to delete
	$include = @("_site", "artifacts", "bin", "obj", "Working", "packages", "TestResults", ".vs", "*.suo", "*.user", "*.orig", "*.dat", "*.lock.json", "*.nuget.props", "*.nuget.targets")
	Write-Host -ForegroundColor Green "Clearing $include"
	
	# Define files and directories to exclude
	$exclude = @()
	
	$items = Get-ChildItem $sourceDir -recurse -force -include $include -exclude $exclude
	$count = $items.Count
	Write-Host -ForegroundColor Green "Removing $count object(s)"
	if ($items) {
		foreach ($item in $items) {
			Remove-Item $item.FullName -Force -Recurse -ErrorAction SilentlyContinue
			Write-Host -ForegroundColor Green "Deleted" $item.FullName
		}
	}
	
	if (Test-Path -path $workingDir)
	{
		Write-Host "Deleting existing working directory $workingDir"
		Execute-Command -command { del $workingDir -Recurse -Force }
	}
	
	Write-Host "Creating working directory $workingDir"
	New-Item -Path $workingDir -ItemType Directory
}

task Build -depends Clean {
	Write-Host "Copying source to working source directory $workingSourceDir"
	robocopy $sourceDir $workingSourceDir /MIR /NP /XD bin obj TestResults packages $packageDirs .vs artifacts /XF *.suo *.user *.lock.json | Out-Default
	
	Write-Host -ForegroundColor Green "Updating assembly version"
	Write-Host
	Update-AssemblyInfoFiles $workingSourceDir ($majorVersion + '.0.0') $version
	
	Write-Host
	Write-Host "Restoring $workingSourceDir\$packageId.sln" -ForegroundColor Green
	[Environment]::SetEnvironmentVariable("EnableNuGetPackageRestore", "true", "Process")
	exec { .\Tools\NuGet\NuGet.exe update -self }
	exec { .\Tools\NuGet\NuGet.exe restore "$workingSourceDir\$packageId.sln" -verbosity detailed -configfile $workingSourceDir\nuget.config | Out-Default } "Error restoring $packageId"
		
	Write-Host
	Write-Host "Building $workingSourceDir\$packageId.sln with docs=$doc" -ForegroundColor Green
	
	exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release /p:OutputPath=bin\Release "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=14.0" "$workingSourceDir\$packageId.sln" | Out-Default } "Error building $packageId"

}

task Test -depends Build {
	
	$name = "$packageId.Tests"
	$finalDir = $build.FinalDir
	$framework = "net-4.0"
	
	Write-Host -ForegroundColor Green "Copying test assembly $name to deployed directory"
	Write-Host
	robocopy "$workingSourceDir\numl.Tests\bin\Release" $workingDir\Deployed\Bin /MIR /NFL /NDL /NJS /NC /NS /NP /XO | Out-Default
	
	Copy-Item -Path "$workingSourceDir\numl.Tests\bin\Release\numl.Tests.dll" -Destination $workingDir\Deployed\Bin\$finalDir\
	
	Write-Host -ForegroundColor Green "Running NUnit tests " $name
	Write-Host
	exec { .\Tools\NUnit\nunit-console.exe "$workingDir\Deployed\Bin\numl.Tests.dll" /result=$workingDir\$name.xml /trace=Info /labels | Out-Default } "Error running $name tests"
}

task DnxBuild -depends Test {
	
	$p = Get-Location
	Set-Location -Path $workingSourceDir\numl
	$name = "$packageId.dotnet"
	rename-item -path "$workingSourceDir\numl\numl.dotnet.project.json" -newname "$workingSourceDir\numl\project.json"
	$projectPath = "$workingSourceDir\numl\project.json"
	
	exec { dnvm install $dnvmVersion -r clr | Out-Default }
	exec { dnvm use $dnvmVersion -r clr | Out-Default }
	
	Write-Host -ForegroundColor Green "Restoring packages for $name"
	Write-Host
	exec { dnu restore $projectPath | Out-Default }
	
	Write-Host -ForegroundColor Green "Building $projectPath"
	exec { dnu build --out $workingDir\DNXBuild --configuration Release  | Out-Default }
	
	New-Item -Path $workingDir\NuGet\lib -ItemType Directory
	robocopy $workingDir\DNXBuild\Release $workingDir\NuGet\lib *.dll *.pdb *.xml /NFL /NDL /NJS /NC /NS /NP /XO /XF /S *.CodeAnalysisLog.xml | Out-Default
	
	Execute-Command -command { del  $workingDir\DNXBuild -Recurse -Force }

	
	Set-Location -Path $p
}

task Nuget -depends DnxBuild {
	
	$nugetVersion = $majorWithReleaseVersion
	if ($nugetPrelease -ne $null)
	{
		$nugetVersion = $nugetVersion + "-" + $nugetPrelease
	}

	If (!(Test-Path $workingDir\NuGet)) {
		New-Item -Path $workingDir\NuGet -ItemType Directory
	}
	
	$nuspecPath = "$workingDir\NuGet\numl.nuspec"
	Copy-Item -Path "$buildDir\numl.nuspec" -Destination $nuspecPath -recurse
	
	Write-Host "Updating nuspec file at $nuspecPath" -ForegroundColor Green
	Write-Host
	
	$xml = [xml](Get-Content $nuspecPath)
	Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'id']" -value $packageId
	Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'version']" -value $nugetVersion
	
	Write-Host $xml.OuterXml
	
	$xml.save($nuspecPath)
		
	robocopy $workingSourceDir $workingDir\NuGet\src *.cs /S /NFL /NDL /NJS /NC /NS /NP /XD numl.Tests obj .vs artifacts | Out-Default
	
	Write-Host "Building NuGet package with ID $packageId and version $nugetVersion" -ForegroundColor Green
	$p = Get-Location
	
	Set-Location -Path $toolsDir
	
	Write-Host
	
	exec { .\NuGet\NuGet.exe pack $nuspecPath -Symbols }
	
	Write-Host
	Write-Host "Moving package to $workingDir\NuGet" -ForegroundColor Green
	move -Path .\*.nupkg -Destination $workingDir\NuGet
	
	Set-Location -Path $p
}

task Docs -depends DnxBuild {
	
	Set-Location $docDir
	exec { dnu commands install docfx }
	exec { docfx --logLevel Verbose }
}
properties {
	$majorVersion = "0.9"
	$majorWithReleaseVersion = "0.9.2"
	$nugetPrelease = "alpha"
	$packageId = "numl"
	$version = GetVersion $majorWithReleaseVersion
	$treatWarningsAsErrors = $true
	$workingName = if ($workingName) {$workingName} else {"Working"}
	$baseDir  = resolve-path ..
	$buildDir = "$baseDir\Build"
	$sourceDir = "$baseDir\Src"
	$toolsDir = "$baseDir\Tools"
	$docDir = "$baseDir\Doc"
	$workingDir = "$baseDir\$workingName"
	$workingSourceDir = "$workingDir\Src"
	$builds = @(
		@{Name = "numl"; TestsName = "numl.Tests"; BuildFunction = "MSBuildBuild"; TestsFunction = "NUnitTests"; FinalDir="net452"; NuGetDir = "net452"; Framework="net-4.0"}
	)
}

framework '4.6x86'
include .\psake_helpers.ps1

task default -depends Nuget

task Clean {
	Write-Host "Setting location to $baseDir"
	Set-Location $baseDir
	
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
	

	foreach ($build in $builds)
	{
		$name = $build.Name
		if ($name -ne $null)
		{
			Write-Host -ForegroundColor Green "Building " $name
			& $build.BuildFunction $build $true
		}
	}
}

task Test -depends Build {
	
	foreach ($build in $builds)
	{
		if ($build.TestsFunction -ne $null)
		{
			& $build.TestsFunction $build $false
		}
	}
}

task Nuget -depends Build {
	foreach ($build in $builds)
	{
		$name = $build.TestsName
		$finalDir = $build.FinalDir
		
		robocopy "$workingSourceDir\numl\bin\Release\$finalDir" $workingDir\Package\Bin\$finalDir *.dll *.pdb *.xml /NFL /NDL /NJS /NC /NS /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
	}
	
	$nugetVersion = $majorWithReleaseVersion
	if ($nugetPrelease -ne $null)
	{
		$nugetVersion = $nugetVersion + "-" + $nugetPrelease
	}

	New-Item -Path $workingDir\NuGet -ItemType Directory
	
	$nuspecPath = "$workingDir\NuGet\numl.nuspec"
	Copy-Item -Path "$buildDir\numl.nuspec" -Destination $nuspecPath -recurse
	
	Write-Host "Updating nuspec file at $nuspecPath" -ForegroundColor Green
	Write-Host
	
	$xml = [xml](Get-Content $nuspecPath)
	Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'id']" -value $packageId
	Edit-XmlNodes -doc $xml -xpath "//*[local-name() = 'version']" -value $nugetVersion
	
	Write-Host $xml.OuterXml
	
	$xml.save($nuspecPath)
	
	foreach ($build in $builds)
	{
		if ($build.NuGetDir)
		{
			$name = $build.TestsName
			$finalDir = $build.FinalDir
			$frameworkDirs = $build.NuGetDir.Split(",")
			
			foreach ($frameworkDir in $frameworkDirs)
			{
				robocopy "$workingSourceDir\numl\bin\Release\$finalDir" $workingDir\NuGet\lib\$frameworkDir *.dll *.pdb *.xml /NFL /NDL /NJS /NC /NS /NP /XO /XF *.CodeAnalysisLog.xml | Out-Default
			}
		}
	}
	
	robocopy $workingSourceDir $workingDir\NuGet\src *.cs /S /NFL /NDL /NJS /NC /NS /NP /XD numl.Tests obj .vs artifacts | Out-Default
	
	Write-Host "Building NuGet package with ID $packageId and version $nugetVersion" -ForegroundColor Green
	Write-Host
	
	exec { .\Tools\NuGet\NuGet.exe pack $nuspecPath -Symbols }
	
	Write-Host
	Write-Host "Moving package to $workingDir\NuGet" -ForegroundColor Green
	move -Path .\*.nupkg -Destination $workingDir\NuGet
}

properties {
	$workingName = if ($workingName) {$workingName} else {"Working"}
	
	$baseDir  = resolve-path ..
	$buildDir = "$baseDir\Build"
	$sourceDir = "$baseDir\Src"
	$toolsDir = "$baseDir\Tools"
	$docDir = "$baseDir\Doc"
	$releaseDir = "$baseDir\Release"
	$workingDir = "$baseDir\$workingName"
}

framework '4.6x86'

task default -depends Test

task Clean {
	Write-Host "Setting location to $baseDir"
	Set-Location $baseDir
	
	if (Test-Path -path $workingDir)
	{
		Write-Host "Deleting existing working directory $workingDir"
	
		Invoke-Command { del $workingDir -Recurse -Force }
	}
	
	Write-Host "Creating working directory $workingDir"
	New-Item -Path $workingDir -ItemType Directory
}

task Build -depends Clean {
	Write-Host "Build step!"
}

task Package -depends Build {
	Write-Host "Package step!"
}

task Test -depends Build {
	Write-Host "Test step!"
}
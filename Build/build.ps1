properties {
	$baseDir  = resolve-path ..
	$buildDir = "$baseDir\Build"
	$sourceDir = "$baseDir\Src"
	$toolsDir = "$baseDir\Tools"
	$docDir = "$baseDir\Doc"
	$releaseDir = "$baseDir\Release"	
}

task default -depends Test

task Clean {
	Write-Host "Clean step!"
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
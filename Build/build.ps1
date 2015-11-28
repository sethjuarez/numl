properties {
	$majorVersion = "1.0"
	$majorWithReleaseVersion = "1.0.1"
	$nugetPrelease = "alpha"
	$version = GetVersion $majorWithReleaseVersion
	$workingName = if ($workingName) {$workingName} else {"Working"}
	$signAssemblies = $false
	$baseDir  = resolve-path ..
	$buildDir = "$baseDir\Build"
	$sourceDir = "$baseDir\Src"
	$toolsDir = "$baseDir\Tools"
	$docDir = "$baseDir\Doc"
	$releaseDir = "$baseDir\Release"
	$workingDir = "$baseDir\$workingName"
	$workingSourceDir = "$workingDir\Src"
	$builds = @(
		@{Name = "numl"; TestsName = "numl.Tests"; BuildFunction = "MSBuildBuild"; TestsFunction = "NUnitTests"; Constants=""; FinalDir="Net45"; NuGetDir = "net45"; Framework="net-4.0"}
	)
}

framework '4.6x86'

task default -depends Test

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
	
	Update-Project $workingSourceDir\numl\project.json $signAssemblies
	
	foreach ($build in $builds)
	{
		$name = $build.Name
		if ($name -ne $null)
		{
			Write-Host -ForegroundColor Green "Building " $name
			Write-Host -ForegroundColor Green "Signed " $signAssemblies
			Write-Host -ForegroundColor Green "Key " $signKeyPath
			
			& $build.BuildFunction $build
		}
	}
}

task Package -depends Build {
	Write-Host "Package step!"
}

task Test -depends Build {
	Write-Host "Test step!"
}

function Update-AssemblyInfoFiles ([string] $workingSourceDir, [string] $assemblyVersionNumber, [string] $fileVersionNumber)
{
	$assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$assemblyVersion = 'AssemblyVersion("' + $assemblyVersionNumber + '")';
	$fileVersion = 'AssemblyFileVersion("' + $fileVersionNumber + '")';
	
	Get-ChildItem -Path $workingSourceDir -r -filter AssemblyInfo.cs | ForEach-Object {
		Write-Host $workingSourceDir
		$filename = $_.Directory.ToString() + '\' + $_.Name
		Write-Host $filename
		$filename + ' -> ' + $version
		
		(Get-Content $filename) | ForEach-Object {
			% {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
			% {$_ -replace $fileVersionPattern, $fileVersion }
			
		} | Set-Content $filename
	}
}

function Update-Project {
  param (
    [string] $projectPath,
    [string] $sign
  )

  $file = switch($sign) { $true { $signKeyPath } default { $null } }

  $json = (Get-Content $projectPath) -join "`n" | ConvertFrom-Json
  $options = @{"warningsAsErrors" = $true; "keyFile" = $file; "define" = ((GetConstants "dotnet" $sign) -split ";") }
  Add-Member -InputObject $json -MemberType NoteProperty -Name "compilationOptions" -Value $options -Force

  ConvertTo-Json $json -Depth 10 | Set-Content $projectPath
}

function GetConstants($constants, $includeSigned)
{
	$signed = switch($includeSigned) { $true { ";SIGNED" } default { "" } }

	return "CODE_ANALYSIS;TRACE;$constants$signed"
}

function MSBuildBuild($build)
{
	$name = $build.Name
	$finalDir = $build.FinalDir
	
	Write-Host
	Write-Host "Restoring $workingSourceDir\$name.sln" -ForegroundColor Green
	[Environment]::SetEnvironmentVariable("EnableNuGetPackageRestore", "true", "Process")
	exec { .\Tools\NuGet\NuGet.exe update -self }
	exec { .\Tools\NuGet\NuGet.exe restore "$workingSourceDir\$name.sln" -verbosity detailed -configfile $workingSourceDir\nuget.config | Out-Default } "Error restoring $name"
	
	$constants = GetConstants $build.Constants $signAssemblies
	
	Write-Host
	Write-Host "Building $workingSourceDir\$name.sln" -ForegroundColor Green
	exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release "/p:CopyNuGetImplementations=true" "/p:Platform=Any CPU" "/p:PlatformTarget=AnyCPU" /p:OutputPath=bin\Release\$finalDir\ /p:AssemblyOriginatorKeyFile=$signKeyPath "/p:SignAssembly=$signAssemblies" "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=14.0" /p:DefineConstants=`"$constants`" "$workingSourceDir\$name.sln" | Out-Default } "Error building $name"
}

function NUnitTests($build)
{
	$name = $build.TestsName
	$finalDir = $build.FinalDir
	$framework = $build.Framework
	
	Write-Host -ForegroundColor Green "Copying test assembly $name to deployed directory"
	Write-Host
	robocopy "$workingSourceDir\numl.Tests\bin\Release\$finalDir" $workingDir\Deployed\Bin\$finalDir /MIR /NFL /NDL /NJS /NC /NS /NP /XO | Out-Default
	
	Copy-Item -Path "$workingSourceDir\numl.Tests\bin\Release\$finalDir\numl.Tests.dll" -Destination $workingDir\Deployed\Bin\$finalDir\
	
	Write-Host -ForegroundColor Green "Running NUnit tests " $name
	Write-Host
	exec { .\Tools\NUnit\nunit-console.exe "$workingDir\Deployed\Bin\$finalDir\numl.Tests.dll" /framework=$framework /xml:$workingDir\$name.xml | Out-Default } "Error running $name tests"
}

function GetVersion($majorVersion)
{
	$now = [DateTime]::Now
	
	$year = $now.Year - 2000
	$month = $now.Month
	$totalMonthsSince2000 = ($year * 12) + $month
	$day = $now.Day
	$minor = "{0}{1:00}" -f $totalMonthsSince2000, $day
	
	$hour = $now.Hour
	$minute = $now.Minute
	$revision = "{0:00}{1:00}" -f $hour, $minute
	
	return $majorVersion + "." + $minor
}

function Execute-Command($command) 
{
	$currentRetry = 0
	$success = $false
	do 
	{
		try
		{
			& $command
			$success = $true
		}
		catch [System.Exception]
		{
			if ($currentRetry -gt 5) 
			{
				throw $_.Exception.ToString()
			} 
			else 
			{
				write-host "Retry $currentRetry"
				Start-Sleep -s 1
			}
			$currentRetry = $currentRetry + 1
		}
	} 
	while (!$success)
}
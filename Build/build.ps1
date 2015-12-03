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
		@{Name = "numl"; TestsName = "numl.Tests"; BuildFunction = "MSBuildBuild"; TestsFunction = "NUnitTests"; Constants=""; FinalDir="Portable"; NuGetDir = "portable-net45+wp80+win8+wpa81+dnxcore50"; Framework="net-4.0"}
	)
}

framework '4.6x86'

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
	
	Update-Project $workingSourceDir\numl\project.json
	
	foreach ($build in $builds)
	{
		$name = $build.Name
		if ($name -ne $null)
		{
			Write-Host -ForegroundColor Green "Building " $name
			& $build.BuildFunction $build
		}
	}
}

task Test -depends Build {
	Update-Project $workingSourceDir\numl\project.json $false

	foreach ($build in $builds)
	{
		if ($build.TestsFunction -ne $null)
		{
			& $build.TestsFunction $build
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
	exec { .\Tools\NUnit\nunit-console.exe "$workingDir\Deployed\Bin\$finalDir\numl.Tests.dll" /result=$workingDir\$name.xml /trace=Info /labels | Out-Default } "Error running $name tests"
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

function Update-Project($projectPath) 
{
	$json = (Get-Content $projectPath) -join "`n" | ConvertFrom-Json
	$options = @{"warningsAsErrors" = $true; "define" = ((GetConstants "dotnet") -split ";") }
	Add-Member -InputObject $json -MemberType NoteProperty -Name "compilationOptions" -Value $options -Force
	ConvertTo-Json $json -Depth 10 | Set-Content $projectPath
}

function GetConstants($constants)
{
	return "CODE_ANALYSIS;TRACE;$constants"
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
	
	$constants = GetConstants $build.Constants
	
	Write-Host
	Write-Host "Building $workingSourceDir\$name.sln" -ForegroundColor Green
	if ($name -match 'Tests') { 
		Write-Host "NO DOCS $name" -ForegroundColor Red
		#exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release /p:OutputPath=bin\Release\$finalDir\ "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=14.0" /p:DefineConstants=`"$constants`" "$workingSourceDir\$name.sln" | Out-Default } "Error building $name"
	} else {
		Write-Host "YES DOCS $name" -ForegroundColor Red
		#exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release /p:OutputPath=bin\Release\$finalDir\ "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=14.0" "/p:documentationFile=bin\Release\$finalDir\$name.xml" /p:DefineConstants=`"$constants`" "$workingSourceDir\$name.sln" | Out-Default } "Error building $name"	
	}
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

function Edit-XmlNodes {
    param (
        [xml] $doc,
        [string] $xpath = $(throw "xpath is a required parameter"),
        [string] $value = $(throw "value is a required parameter")
    )
    
    $nodes = $doc.SelectNodes($xpath)
    $count = $nodes.Count

    Write-Host "Found $count nodes with path '$xpath'"
    
    foreach ($node in $nodes) {
        if ($node -ne $null) {
            if ($node.NodeType -eq "Element")
            {
                $node.InnerXml = $value
            }
            else
            {
                $node.Value = $value
            }
        }
    }
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
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

function GetConstants($constants)
{
	return "CODE_ANALYSIS;TRACE;$constants"
}

function MSBuildBuild($build, $doc)
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
	Write-Host "Building $workingSourceDir\$name.sln with docs=$doc" -ForegroundColor Green
	
	if($doc) {
		exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release /p:OutputPath=bin\Release\$finalDir\ "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=14.0" "/p:documentationFile=bin\Release\$finalDir\$name.xml" /p:DefineConstants=`"$constants`" "$workingSourceDir\$name.sln" | Out-Default } "Error building $name"
	} else {
		exec { msbuild "/t:Clean;Rebuild" /p:Configuration=Release /p:OutputPath=bin\Release\$finalDir\ "/p:TreatWarningsAsErrors=$treatWarningsAsErrors" "/p:VisualStudioVersion=14.0" /p:DefineConstants=`"$constants`" "$workingSourceDir\$name.sln" | Out-Default } "Error building $name"
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

cls
powershell -Command "& { [Console]::WindowWidth = 150; [Console]::WindowHeight = 40; Start-Transcript %~dp0rundocs.txt; Import-Module %~dp0..\Tools\PSake\psake.psm1; Invoke-psake %~dp0..\Build\docs.ps1 %*; Stop-Transcript; }"

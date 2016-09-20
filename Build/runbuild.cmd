cls
powershell -Command "& { Set-ExecutionPolicy RemoteSigned; Start-Transcript %~dp0runbuild.txt; Invoke-Expression %~dp0build.ps1; } "
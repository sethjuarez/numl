# Set Path
$baseDir  = resolve-path ..
$sourceDir = "$baseDir\Src"

Write-Host "Searching $sourceDir"

# Define files and directories to delete
$include = @("bin", "obj", "Working", "packages", "TestResults", ".vs", "*.suo", "*.user", "*.orig", "*.dat", "*.lock.json", "*.nuget.props", "*.nuget.targets")
Write-Host -ForegroundColor Green "Clearing $include"

# Define files and directories to exclude
$exclude = @()

$items = Get-ChildItem $sourceDir -recurse -force -include $include -exclude $exclude

if ($items) {
    foreach ($item in $items) {
        Remove-Item $item.FullName -Force -Recurse -ErrorAction SilentlyContinue
        Write-Host -ForegroundColor Red "Deleted" $item.FullName
    }
}

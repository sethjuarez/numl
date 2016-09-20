#tool "nuget:?package=xunit.runner.console"
#tool "docfx.msbuild"
#addin "Newtonsoft.Json"
#addin "Cake.DocFx"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var release = "0.9.9";
var suffix = "-beta";
var testFailOk = true;
var copyright = string.Format("©{0}, Seth Juarez", DateTime.Now.Year);
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./Output") + Directory(configuration);
var testResultsDir = Directory("./Output/Results");
var packageDir = Directory("./Output/Package");

//////////////////////////////////////////////////////////////////////
// Utility
//////////////////////////////////////////////////////////////////////
// utility function that patches project.json using json.net
public static void UpdateProjectJsonVersion(string version, FilePath projectPath)
{
    var project = Newtonsoft.Json.Linq.JObject.Parse(
        System.IO.File.ReadAllText(projectPath.FullPath, Encoding.UTF8));
    project["version"].Replace(version);
    System.IO.File.WriteAllText(projectPath.FullPath, project.ToString(), Encoding.UTF8);
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    CleanDirectory(testResultsDir);
    CleanDirectory(packageDir);
});

Task("Version")
    .IsDependentOn("Clean")
    .Does(() =>
{
    var assemblyInfo = new AssemblyInfoSettings
    {
        Product = "numl",
        Description = "numl is a machine learning library intended to ease the use of using standard modeling techniques for both prediction and clustering",
        Guid = "554363c6-5979-4c9a-90e6-e70af2d5cc09",
        Version = release,
        FileVersion = release,
        ComVisible = false,
        Copyright = copyright
    };
    // update assembly version
    CreateAssemblyInfo("../Src/numl/Properties/AssemblyInfo.cs", assemblyInfo);
    // update project.json build
    UpdateProjectJsonVersion(release + suffix, "../Src/numl/project.json");
});

Task("Restore")
    .IsDependentOn("Version")
    .Does(() =>
{
    DotNetCoreRestore("../Src");
});

Task("Test")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreTest("../Src/numl.Tests", new DotNetCoreTestSettings {
        Configuration = configuration,
        Framework = "netcoreapp1.0",
        OutputDirectory = buildDir,
        Verbose = true
    });
})
.OnError(exception => 
{
    if(!testFailOk)
        throw exception;
});

Task("Package")
    .IsDependentOn("Test")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        Configuration = configuration,
        OutputDirectory = packageDir
    };
            
    DotNetCorePack("../Src/numl", settings);
});

Task("Docs")
    .Does(() => 
{
    DocFx("../Docs/docfx.json", new DocFxSettings()
    {
        OutputPath = "./Output/Docs"
    });

    CopyFile("../Docs/index.html", "./Output/docs/_site/index.html");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////
Task("Default")
    .IsDependentOn("Package");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);

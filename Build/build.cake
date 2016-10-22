#tool "nuget:?package=xunit.runner.console"
#tool "docfx.msbuild"
#addin "Newtonsoft.Json"
#addin "Cake.DocFx"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var release = "0.9.12";
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
public static void UpdateProjectJsonVersion(string version, FilePath projectPath, string node)
{
    var project = Newtonsoft.Json.Linq.JObject.Parse(
        System.IO.File.ReadAllText(projectPath.FullPath, Encoding.UTF8));

    if (node.Contains("+"))
    {
        // for nested entries use a "+"
        var entries = node.Split('+');
        var n = project[entries[0]];
        for (int i = 1; i < entries.Length; i++)
            n = n[entries[i]];
        n.Replace(version);
    }
    else
        project[node].Replace(version);

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
    Information("Updating AssembyInfo");
    CreateAssemblyInfo("../Src/numl/Properties/AssemblyInfo.cs", assemblyInfo);
    // update project.json build
    Information("Updating numl project.json to " + release+suffix);
    UpdateProjectJsonVersion(release + suffix, "../Src/numl/project.json", "version");
    // update test assembly project.json to match new version
    Information("Updating numl.Tests reference to numl in project.json to " + release+suffix);
    UpdateProjectJsonVersion(release + suffix, "../Src/numl.Tests/project.json", "dependencies+numl");
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
    .IsDependentOn("Package")
    .Does(() => 
{
    // write out version in prep for doc gen
    UpdateProjectJsonVersion(release + suffix, "../Docs/version.json", "_appId");

    DocFx("../Docs/docfx.json", new DocFxSettings()
    {
        OutputPath = "./Output/Docs"
    });

    // move to site repo
    CopyDirectory("./Output/docs/_site", "../../numl.web");
    
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////
Task("Default")
    .IsDependentOn("Docs");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);

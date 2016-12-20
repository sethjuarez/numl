#tool "nuget:?package=xunit.runner.console"
#addin "Newtonsoft.Json"
#addin "Cake.DocFx"
#tool "docfx.console"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var release = "0.9.13";
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

public static void UpdateCsProjNode(FilePath csprojPath, string xpath, string contents)
{
    var doc = new System.Xml.XmlDocument();
    doc.Load(csprojPath.FullPath);
    var node = doc.SelectSingleNode(xpath);
    if(node != null)
    {
        node.InnerText = contents;
        doc.Save(csprojPath.FullPath);
    }
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
    // update csproj build
    Information("Updating numl project file");
    UpdateCsProjNode("../Src/numl/numl.csproj", "//PropertyGroup/VersionPrefix", release+suffix);
});

Task("Restore")
    .IsDependentOn("Version")
    .Does(() =>
{
    
    DotNetCoreRestore("../src/numl/numl.csproj");
    DotNetCoreRestore("../src/numl.Tests/numl.Tests.csproj");
});

Task("Test")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreTest("../Src/numl.Tests/numl.Tests.csproj", new DotNetCoreTestSettings {
        Configuration = configuration,
        Framework = "netcoreapp1.1",
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
            
    DotNetCorePack("../Src/numl/numl.csproj", settings);
});

Task("Docs")
    //.IsDependentOn("Package")
    .Does(() => 
{
    // write out version in prep for doc gen
    UpdateProjectJsonVersion(release + suffix, "../Docs/version.json", "_appId");


    //DocFxMetadata("../Docs/docfx.json");

    DocFx("../Docs/docfx.json", new DocFxSettings()
    {
        OutputPath = "./Output/Docs"
    });

    // move to site repo
    //CopyDirectory("./Output/docs/_site", "../../numl.web");
    
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

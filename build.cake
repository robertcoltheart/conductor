#addin "nuget:?package=Cake.DocFx&version=0.1.6"

#tool "nuget:?package=GitVersion.CommandLine&version=3.5.4"
#tool "nuget:?package=NUnit.ConsoleRunner&version=3.2.1"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
//////////////////////////////////////////////////////////////////////
var version = "1.0.0";

var artifacts = Directory("./artifacts");
var solution = File("./src/Conductor.sln");
var testResults = artifacts + File("TestResults.xml");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Clean")
    .Does(() => 
{
    CleanDirectories("./src/**/bin");
    CleanDirectories("./src/**/obj");

    DeleteFiles("./docs/api/*.yml");
    DeleteFiles("./docs/api/.manifest");

    if (DirectoryExists(artifacts))
        DeleteDirectory(artifacts, true);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => 
{
    NuGetRestore(solution);
});

Task("Versioning")
    .IsDependentOn("Clean")
    .Does(() => 
{
    var result = GitVersion(new GitVersionSettings
    {
        UpdateAssemblyInfo = true
    });

    version = result.NuGetVersion;
});

Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("Versioning")
    .Does(() => 
{
    CreateDirectory(artifacts);

    DotNetBuild(solution, x => 
    {
        x.SetConfiguration("Release");
        x.WithProperty("GenerateDocumentation", "true");
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    NUnit3("./src/**/bin/**/Release/*.Tests.dll", new NUnit3Settings
    {
        Results = testResults
    });
    
    //if (BuildSystem.IsRunningOnAppVeyor)
    //    AppVeyor.UploadTestResults(testResults);

    if (BuildSystem.IsRunningOnAppVeyor)
    {
        var baseUri = EnvironmentVariable("APPVEYOR_URL").TrimEnd('/');
        var url = string.Format("{0}/api/testresults/nunit3/{1}", baseUri, AppVeyor.Environment.JobId);
        
        using (var r = new System.Net.WebClient())
        {
            r.UploadFile(url, testResults);
        }        
    }
});

Task("Package")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .Does(() => 
{
    NuGetPack("./build/Conductor.nuspec", new NuGetPackSettings
    {
        Version = version,
        BasePath = "./src",
        OutputDirectory = artifacts
    });
});

Task("Publish")
    .IsDependentOn("Package")
    .Does(() =>
{
    var package = "./artifacts/Conductor." + version + ".nupkg";

    NuGetPush(package, new NuGetPushSettings
    {
        ApiKey = "API key"
    });
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

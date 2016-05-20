#addin "Cake.DocFx"
#addin "Cake.ReSharperReports"

#tool "docfx.msbuild"
#tool "JetBrains.ReSharper.CommandLineTools"
#tool "NUnit.ConsoleRunner"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
//////////////////////////////////////////////////////////////////////
var isTeamCity = BuildSystem.IsRunningOnTeamCity;
var isAppVeyor = BuildSystem.IsRunningOnAppVeyor;
var isTravis = BuildSystem.IsRunningOnTravisCI;

var buildNumber = isTeamCity ? EnvironmentVariable("BUILD_NUMBER")
                : isAppVeyor ? BuildSystem.AppVeyor.Environment.Build.Number.ToString()
				: isTravis ? BuildSystem.TravisCI.Environment.Build.BuildNumber.ToString()
				: "0"
				?? "0";
				
var version = System.IO.File.ReadAllText("./VERSION").Trim() + "." + buildNumber;

var artifacts = Directory("./artifacts");
var solution = File("./Conductor.sln");
var testResults = artifacts + File("TestResults.xml");
var metricsResults = artifacts + File("MetricsResults.xml");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////
Task("Clean")
	.Does(() => 
{
	CleanDirectories("./src/**/bin");
	CleanDirectories("./src/**/obj");
	
	if (DirectoryExists(artifacts))
		DeleteDirectory(artifacts, true);
});

Task("Restore")
	.IsDependentOn("Clean")
	.Does(() => 
{
	NuGetRestore(solution);
});

Task("AssemblyInfo")
	.IsDependentOn("Clean")
	.Does(() => 
{
	var file = "./SharedAssemblyInfo.cs";
	
	var info = ParseAssemblyInfo(file);
	
	CreateAssemblyInfo(file, new AssemblyInfoSettings
	{
		Product = info.Product,
		Company = info.Company,
		Copyright = info.Copyright,
		Trademark = info.Trademark,
		Version = version,
		FileVersion = version,
		InformationalVersion = version
	});
});

Task("Build")
	.IsDependentOn("Restore")
	.IsDependentOn("AssemblyInfo")
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
	NUnit3("./src/**/bin/**/Release/*.Tests.dll", new NUnit3Settings()
    {
        Results = testResults
    });
    
    if (isAppVeyor)
    {
        var environment = BuildSystem.AppVeyor.Environment;
        var baseUri = EnvironmentVariable("APPVEYOR_URL").TrimEnd('/');
        string url = string.Format("{0}/api/testresults/nunit3/{1}", baseUri, environment.JobId);
        
        Information("Uploading test results to " + url);
        
        //using (var webClient = new System.Net.WebClient())
        //    webClient.UploadFile(url, testResults);
    }
});

Task("Inspect")
	.IsDependentOn("Build")
	.Does(() => 
{
    if (IsRunningOnUnix())
    {
        Information("Skipping code inspection, as ReSharper does not support *nix.");
    }
    else
    {
        InspectCode(solution, new InspectCodeSettings()
        {
            SolutionWideAnalysis = true,
            Profile = "./Conductor.sln.DotSettings",
            OutputFile = metricsResults,
            ThrowExceptionOnFindingViolations = false
        });
    }
});

Task("Package")
	.IsDependentOn("Test")
	.IsDependentOn("Inspect")
	.Does(() => 
{
	NuGetPack("./Conductor.nuspec", new NuGetPackSettings()
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
	
	Information("TODO: Publish to NuGet with NuGetPush");
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


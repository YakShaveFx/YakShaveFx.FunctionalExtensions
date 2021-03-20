using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter(Name = "NUGET_API_KEY")] readonly string NuGetApiKey;

    [Parameter(Name = "FEEDZ_API_KEY")] readonly string FeedzApiKey;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Pack => _ => _
        .DependsOn(Clean, Compile)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution)
                .SetOutputDirectory(ArtifactsDirectory)
                .SetIncludeSymbols(true)
                .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            switch (GitRepository.Branch?.Replace("refs/heads/", ""))
            {
                case "main":
                    DotNetNuGetPush(s => s
                        .SetTargetPath($"{ArtifactsDirectory}/**/*.nupkg")
                        .SetSource("https://api.nuget.org/v3/index.json")
                        .SetApiKey(NuGetApiKey)
                    );
                    break;
                case "develop":
                case "publish":
                    // DotNetNuGetPush(s => s
                    //     .SetTargetPath($"{ArtifactsDirectory}/**/*.nupkg")
                    //     .SetSource("https://f.feedz.io/yakshavefx/functionalextensions/nuget/index.json")
                    //     .SetApiKey(FeedzApiKey)
                    // );
                    //
                    // DotNetNuGetPush(s => s
                    //     .SetTargetPath($"{ArtifactsDirectory}/**/*.snupkg")
                    //     .SetSource("https://f.feedz.io/yakshavefx/functionalextensions/symbols")
                    //     .SetApiKey(FeedzApiKey)
                    // );
                    DotNetNuGetPush(s => s
                        .SetTargetPath($"{ArtifactsDirectory}/**/*.nupkg")
                        .SetSource("https://f.feedz.io/yakshavefx/functionalextensions/nuget/index.json")
                        .SetSymbolSource("https://f.feedz.io/yakshavefx/functionalextensions/symbols")
                        .SetApiKey(FeedzApiKey)
                    );
                    break;
                default:
                    throw new InvalidOperationException($"Current branch \"{GitRepository.Branch}\" should not be publishing packages!");
            }

            
        });

    (string feedUrl, string symbolsFeedUrl, string apiKey) GetPublishTargetSettings()
        => GitRepository.Branch?.Replace("refs/heads/", "") switch
        {
            "main" => (
                "https://api.nuget.org/v3/index.json",
                "https://api.nuget.org/v3/index.json",
                NuGetApiKey),

            "publish" => (
                "https://f.feedz.io/yakshavefx/functionalextensions/nuget/index.json",
                "https://f.feedz.io/yakshavefx/functionalextensions/symbols",
                FeedzApiKey),

            _ => throw new InvalidOperationException(
                $"Current branch \"{GitRepository.Branch}\" should not be publishing packages!")
        };
}
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;
using System.Collections.Generic;

[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Pack);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Azure pipelines build artifacts staging directory")]
    readonly AbsolutePath BuildArtifactStagingDirectory;

    [Solution] 
    readonly Solution Solution;

    [GitVersion(Framework = "net7.0")]
    readonly GitVersion GitVersion;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath ArtifactsDirectory => BuildArtifactStagingDirectory ?? RootDirectory / "artifacts";
    AbsolutePath ArtifactFilesDirectory => ArtifactsDirectory / "files";
    AbsolutePath ArtifactPackagesDirectory => ArtifactsDirectory / "packages";

    // =================================================
    // Clean
    // =================================================

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    // =================================================
    // Compile
    // =================================================

    Target Restore => _ => _
        .DependsOn(Clean)
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
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    // =================================================
    // Prepare
    // =================================================

    private void CopyUIFiles()
    {
        // Copy UI files
        CopyDirectoryRecursively(SourceDirectory / "Vendr.Checkout" / "Web" / "UI", ArtifactFilesDirectory);

        // Copy package assets
        CopyDirectoryRecursively(RootDirectory / "assets", ArtifactFilesDirectory / "assets");

        // Copy readme
        CopyFileToDirectory(RootDirectory / "LICENSE.md", ArtifactFilesDirectory);
    }

    Target Prepare => _ => _
        .DependsOn(Compile)
        .Produces(ArtifactFilesDirectory)
        .Executes(() =>
        {
            CopyUIFiles();
        });

    // =================================================
    // Pack
    // =================================================

    private void PackNuGetPackages()
    {
        var nuSpecsDir = BuildProjectDirectory / "NuGet";
        var nuSpecsFile = nuSpecsDir / $"Vendr.Checkout.nuspec";

        // Package NuGet package
        NuGetPack(c => c
            .SetTargetPath(nuSpecsFile)
            .SetConfiguration(Configuration)
            .SetVersion(GitVersion.NuGetVersionV2)
            .SetProperties(new Dictionary<string, object>
            {
                { "Configuration", Configuration.ToString() },
                { "ProjectDirectory", SourceDirectory / "Vendr.Checkout" },
                { "ArtifactFilesDirectory", ArtifactFilesDirectory }
            })
            .SetOutputDirectory(ArtifactPackagesDirectory));
    }

    Target Pack => _ => _
        .DependsOn(Prepare)
        .Produces(ArtifactPackagesDirectory)
        .Executes(() =>
        {
            PackNuGetPackages();
        });

}

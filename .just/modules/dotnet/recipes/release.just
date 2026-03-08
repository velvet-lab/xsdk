# Publish NuGet packages to registry, requires API key as argument
[no-cd]
[group('deployment')]
publish solution: (build solution)
    #!{{shebang}}
    Remove-Item -Path "{{dist_dir}}/nuget" -Recurse -Force -ErrorAction SilentlyContinue
    $version = $(Get-Content .\package.json | ConvertFrom-Json).version
    $versionSuffix = ""
    try {
        $versionSuffix = $version.Split("-")[1]
        $version = $version.Split("-")[0]
        if ([string]::IsNullOrEmpty($versionSuffix) -eq $false) {
            $versionSuffix = "--version-suffix=$versionSuffix"
        }
    }
    catch {}

    dotnet pack "{{solution}}" --configuration RELEASE --nologo --no-build --no-restore --output "{{dist_dir}}/nuget" $versionSuffix

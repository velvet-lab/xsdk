# Repository - Justfile
# Command runner for development tasks
# See: https://just.systems/man/en/

set shell := ["bash", "-c"]
set windows-shell := ["pwsh.exe", "-NoProfile", "-NoLogo", "-CommandWithArgs"]
set dotenv-load := false
set quiet := true

import 'recipes/module.just'
import 'recipes/maintenance.just'
import 'recipes/lint.just'
import 'recipes/format.just'
import 'recipes/development.just'
import 'recipes/clean.just'

[private]
default:
    @just --list

# Publish NuGet packages to registry, requires API key as argument
[no-cd]
[group('deployment')]
publish solution token: (build solution)
    #!{{shebang}}
    $nuget_host_url="https://api.nuget.org/v3/index.json"
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
    dotnet nuget push --skip-duplicate --api-key "{{token}}" --source "${nuget_host_url}" "{{dist_dir}}/nuget/*.nupkg"

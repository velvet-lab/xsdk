# Repository - Justfile
# Command runner for development tasks
# See: https://just.systems/man/en/

set shell := ["bash", "-c"]
set windows-shell := ["pwsh.exe", "-NoProfile", "-NoLogo", "-CommandWithArgs"]
set dotenv-load := false
set quiet := true

import 'recipes/module.just'

[private]
default:
    @just --list

# Initialize repository with pnpm, write package.json and add needed dependencies
[no-cd]
install:
    @just {{module_name}}::info "Initializing repository '{{repo_name}}'..."
    dotnet tool restore
    @just {{module_name}}::success " '{{repo_name}}'..."

# Clean solution by removing build artifacts and obj folders
[no-cd]
clean solution:
    @just {{module_name}}::info "Cleaning solution '{{solution}}'..."
    dotnet clean "{{solution}}"

    @just {{module_name}}::info "Removing all obj folders..."
    Get-ChildItem -Path "{{repo_root}}" -Filter "obj" -Recurse | Select-Object -ExpandProperty FullName | Remove-Item -Recurse -Force

    @just {{module_name}}::success "Solution '{{solution}}' cleaned successfully."

# Restore solution by downloading all dependencies
[no-cd]
restore solution:
    @just {{module_name}}::info "Restoring solution '{{solution}}'..."
    dotnet restore "{{solution}}"
    @just {{module_name}}::success "Solution '{{solution}}' restored successfully."

# Build whole solution
[no-cd]
build solution: (restore solution)
    @just {{module_name}}::info "Building solution '{{solution}}'..."
    dotnet build "{{solution}}" --configuration Release --nologo --no-restore
    @just {{module_name}}::success "Solution '{{solution}}' built successfully."

# Build solution with code coverage for SonarCloud analysis
[no-cd]
sonar-build solution: (restore solution)
    dotnet build "{{solution}}" --configuration DEBUG --no-incremental --nologo --no-restore
    dotnet dotnet-coverage collect "dotnet test {{solution}} --no-build --no-restore --nologo" --output-format xml --output "coverage.xml" --nologo

# Test whole solution
[no-cd]
test solution:
    @just {{module_name}}::info "Running tests for solution '{{solution}}'..."
    dotnet test "{{solution}}" --configuration Release
    @just {{module_name}}::success "Tests for solution '{{solution}}' completed successfully."

# Publish NuGet packages to registry, requires API key as argument
[no-cd]
publish solution token: (build solution)
    #!{{shebang}}
    $nuget_host_url=""
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

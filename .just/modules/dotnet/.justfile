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

# Restores .NET tools defined in the repository
[no-cd]
[group('maintenance')]
install:
    @just {{module_name}}::info "Initializing repository '{{repo_name}}'..."
    dotnet tool restore
    @just {{module_name}}::success " '{{repo_name}}'..."

# Update .Net tools defined in the repository
[no-cd]
[group('maintenance')]
update:
    @just {{module_name}}::info "Initializing repository '{{repo_name}}'..."
    dotnet tool restore
    @just {{module_name}}::success " '{{repo_name}}'..."

# Clean solution by removing build artifacts and obj folders
[no-cd]
[group('maintenance')]
clean solution:
    @just {{module_name}}::info "Cleaning solution '{{solution}}'..."
    dotnet clean "{{solution}}"

    @just {{module_name}}::info "Removing all obj folders..."
    Get-ChildItem -Path "{{repo_root}}" -Filter "obj" -Recurse | Select-Object -ExpandProperty FullName | Remove-Item -Recurse -Force

    @just {{module_name}}::success "Solution '{{solution}}' cleaned successfully."

# Restore solution by downloading all dependencies
[no-cd]
[group('development')]
restore solution:
    @just {{module_name}}::info "Restoring solution '{{solution}}'..."
    dotnet restore "{{solution}}"
    @just {{module_name}}::success "Solution '{{solution}}' restored successfully."

# Build solution in Release configuration without restoring dependencies
[no-cd]
[group('development')]
build solution: (restore solution)
    @just {{module_name}}::info "Building solution '{{solution}}'..."
    dotnet build "{{solution}}" --configuration Release --nologo --no-restore
    @just {{module_name}}::success "Solution '{{solution}}' built successfully."

# Run tests for solution in Release configuration without building the solution
[no-cd]
[group('development')]
test solution: (build solution)
    @just {{module_name}}::info "Running tests for solution '{{solution}}'..."
    dotnet test "{{solution}}" --configuration Release
    @just {{module_name}}::success "Tests for solution '{{solution}}' completed successfully."

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

# Check code formatting in solution using dotnet format and auto-fix formatting issues
[no-cd]
[group('format')]
format solution:
    @just {{module_name}}::info "Formatting code in solution '{{solution}}'..."
    dotnet format "{{solution}}" --verbosity diagnostic
    @just {{module_name}}::success "Code formatting completed for solution '{{solution}}'."

# Check code formatting in solution using dotnet format with --verify-no-changes flag
[no-cd]
[group('format')]
check-format solution:
    @just {{module_name}}::info "Checking code formatting for solution '{{solution}}'..."
    dotnet format "{{solution}}" --verify-no-changes --verbosity diagnostic
    @just {{module_name}}::success "Code formatting check completed for solution '{{solution}}'."

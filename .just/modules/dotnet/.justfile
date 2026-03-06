# Repository - Justfile
# Command runner for development tasks
# See: https://just.systems/man/en/

set shell := ["bash", "-c"]
set windows-shell := ["pwsh.exe", "-NoProfile", "-NoLogo", "-CommandWithArgs"]
set dotenv-load := false
set quiet := true

import 'recipes/module.just'
import 'recipes/repository.just'

[private]
default:
    @just --list

# Clean solution by removing build artifacts and obj folders
[no-cd]
clean solution:
    @just {{module_name}}::info "Cleaning solution '{{solution}}'..."
    dotnet clean "{{solution}}"

    @just {{module_name}}::info "Removing all obj folders..."
    Get-ChildItem -Path "{{repo_root}}" -Filter "obj" -Recurse | Select-Object -ExpandProperty FullName | Remove-Item -Recurse -Force

    @just {{module_name}}::success "Solution '{{solution}}' cleaned successfully."

# Build whole solution
[no-cd]
build solution:
    @just {{module_name}}::info "Building solution '{{solution}}'..."
    dotnet build "{{solution}}" --configuration Release
    @just {{module_name}}::success "Solution '{{solution}}' built successfully."

# Build solution with code coverage for SonarCloud analysis
[no-cd]
sonar-build solution:
    dotnet build "{{solution}}" --configuration DEBUG --no-incremental --nologo --no-restore
    dotnet dotnet-coverage collect "dotnet test {{solution}} --no-build --no-restore --nologo" --output-format xml --output "coverage.xml" --nologo

# Test whole solution
[no-cd]
test solution:
    @just {{module_name}}::info "Running tests for solution '{{solution}}'..."
    dotnet test "{{solution}}" --configuration Release
    @just {{module_name}}::success "Tests for solution '{{solution}}' completed successfully."

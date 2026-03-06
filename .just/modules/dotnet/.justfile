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

[no-cd]
build solution:
    @just {{module_name}}::info "Building solution '{{solution}}'..."
    dotnet build "{{solution}}" --configuration Release
    @just {{module_name}}::success "Solution '{{solution}}' built successfully."

[no-cd]
test solution:
    @just {{module_name}}::info "Running tests for solution '{{solution}}'..."
    dotnet test "{{solution}}" --configuration Release
    @just {{module_name}}::success "Tests for solution '{{solution}}' completed successfully."

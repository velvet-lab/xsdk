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

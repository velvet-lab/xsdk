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

# Specific build for SonarQube analysis with code coverage collection
[no-cd]
[group('development')]
sonar-build solution:
    # REMARKS: Its ok, that dotnet restore is not run here. For clean results in sonar scan, we need to ensure
    # that all dependencies are restored and the build is successful before running the scan. This is because
    # Sonar Scanner analyzes the compiled code and needs all dependencies to be in place to provide accurate results.
    # Restore will be triggered by GitHub workflow before sonar scan, so we can be sure that all dependencies are in place.
    # Otherwise, if restore is not run, a error is ok
    @just {{module_name}}::info "Building solution '{{solution}}' for SonarQube analysis..."
    dotnet build "{{solution}}" --configuration Debug --no-incremental --nologo --no-restore
    dotnet dotnet-coverage collect "dotnet test "{{solution}}" --no-build --no-restore --nologo" --output-format xml --output coverage.xml --nologo
    @just {{module_name}}::success "Solution '{{solution}}' built successfully for SonarQube analysis."

# Run tests for solution in Release configuration without building the solution
[no-cd]
[group('development')]
test solution: (build solution)
    @just {{module_name}}::info "Running tests for solution '{{solution}}'..."
    dotnet test "{{solution}}" --configuration Release --no-build --verbosity normal --logger "trx;LogFileName=test-results.trx"
    @just {{module_name}}::success "Tests for solution '{{solution}}' completed successfully."

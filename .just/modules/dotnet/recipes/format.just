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

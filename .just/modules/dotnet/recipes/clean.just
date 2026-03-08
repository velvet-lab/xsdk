# Clean solution by removing build artifacts and obj folders
[no-cd]
[group('maintenance')]
clean solution: (_clean solution)

[private]
[no-cd]
[no-exit-message]
[windows]
_clean solution:
    @just {{module_name}}::info "Cleaning solution '{{solution}}'..."
    dotnet clean "{{solution}}"

    @just {{module_name}}::info "Removing all obj folders..."
    Get-ChildItem -Path "{{repo_root}}" -Filter "obj" -Recurse | Select-Object -ExpandProperty FullName | Remove-Item -Recurse -Force
    Get-ChildItem -Path "{{repo_root}}" -Filter "TestResults" -Recurse | Select-Object -ExpandProperty FullName | Remove-Item -Recurse -Force

    @just {{module_name}}::success "Solution '{{solution}}' cleaned successfully."

[private]
[no-cd]
[no-exit-message]
[unix]
_clean solution:
    @just {{module_name}}::info "Cleaning solution '{{solution}}'..."
    dotnet clean "{{solution}}"

    @just {{module_name}}::info "Removing all obj folders..."
    find "{{repo_root}}" -type d -name "obj" -exec rm -rf {} +
    find "{{repo_root}}" -type d -name "TestResults" -exec rm -rf {} +

    @just {{module_name}}::success "Solution '{{solution}}' cleaned successfully."

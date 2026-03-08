# Restores .NET tools defined in the repository
[no-cd]
[group('maintenance')]
install:
    @just {{module_name}}::info "Initializing repository '{{repo_name}}'..."
    dotnet tool restore
    @just {{module_name}}::success "Tools restored for repository '{{repo_name}}'"

# Update .Net tools defined in the repository
[no-cd]
[group('maintenance')]
update:
    @just {{module_name}}::info "Updating tools for repository '{{repo_name}}'..."
    dotnet tool update --tool-manifest --all
    @just {{module_name}}::success "Tools updated for repository '{{repo_name}}'"

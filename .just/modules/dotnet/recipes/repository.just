
# Initialize repository with pnpm, write package.json and add needed dependencies
[no-cd]
install:
    @just {{module_name}}::info "Initializing repository '{{repo_name}}'..."
    dotnet tool restore
    @just {{module_name}}::success " '{{repo_name}}'..."

# Validate repository using MegaLinter for flavor dotnet
[no-cd]
[group('linting')]
check-lint:
    @just {{module_name}}::info "Linting repository '{{repo_name}}'..."
    pnpm mega-linter-runner --flavor dotnetweb
    @just {{module_name}}::success "Linting completed for repository '{{repo_name}}'."

# Autofix linting issues in repository using MegaLinter for flavor dotnet
[no-cd]
[group('linting')]
lint:
    @just {{module_name}}::info "Linting with auto-fix for repository '{{repo_name}}'..."
    pnpm mega-linter-runner --flavor dotnetweb --fix
    @just {{module_name}}::success "Linting with auto-fix completed for repository '{{repo_name}}'."

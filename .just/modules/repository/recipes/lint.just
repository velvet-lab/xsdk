# Lint repository using MegaLinter
[no-cd]
[group('linting')]
check-lint: install
    @just {{module_name}}::info "Linting repository '{{repo_name}}'..."
    pnpm mega-linter-runner --image oxsecurity/megalinter:latest
    @just {{module_name}}::success "Linting completed for repository '{{repo_name}}'."

# Lint repository with auto-fix using MegaLinter
[no-cd]
[group('linting')]
lint: install
    @just {{module_name}}::info "Linting with auto-fix for repository '{{repo_name}}'..."
    pnpm mega-linter-runner --image oxsecurity/megalinter:latest --fix
    @just {{module_name}}::success "Linting with auto-fix completed for repository '{{repo_name}}'."

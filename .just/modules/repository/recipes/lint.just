# Lint repository using MegaLinter
[no-cd]
lint:
    @just {{module_name}}::info "Linting repository '{{repo_name}}'..."
    pnpm mega-linter-runner --image oxsecurity/megalinter:latest
    @just {{module_name}}::success "Linting completed for repository '{{repo_name}}'."

# Lint repository with auto-fix using MegaLinter
[no-cd]
lint-fix:
    @just {{module_name}}::info "Linting with auto-fix for repository '{{repo_name}}'..."
    pnpm mega-linter-runner --image oxsecurity/megalinter:latest --fix
    @just {{module_name}}::success "Linting with auto-fix completed for repository '{{repo_name}}'."


[no-cd]
format:
    @just {{module_name}}::info "Formatting code in repository '{{repo_name}}'..."
    pnpm prettier --write "**/*.{json,md,yml,yaml,js,ts}"
    @just {{module_name}}::success "Code formatting completed for repository '{{repo_name}}'."

[no-cd]
check-format:
    @just {{module_name}}::info "Checking code formatting for repository '{{repo_name}}'..."
    pnpm prettier --check "**/*.{json,md,yml,yaml,js,ts}"
    @just {{module_name}}::success "Code formatting check completed for repository '{{repo_name}}'."

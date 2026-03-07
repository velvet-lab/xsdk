# Format code in repository using Prettier
[no-cd]
[group('format')]
format:
    @just {{module_name}}::info "Formatting code in repository '{{repo_name}}'..."
    pnpm prettier --write "**/*.{json,md,yml,yaml,js,ts}"
    @just {{module_name}}::success "Code formatting completed for repository '{{repo_name}}'."

# Check code formatting in repository using Prettier
[no-cd]
[group('format')]
check-format:
    @just {{module_name}}::info "Checking code formatting for repository '{{repo_name}}'..."
    pnpm prettier --check "**/*.{json,md,yml,yaml,js,ts}"
    @just {{module_name}}::success "Code formatting check completed for repository '{{repo_name}}'."

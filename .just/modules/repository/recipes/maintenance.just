# Install repository dependencies with pnpm
[no-cd]
[group('maintenance')]
install:
    @just {{module_name}}::info "Installing dependencies for repository '{{repo_name}}'..."
    pnpm install
    @just {{module_name}}::success "Dependencies installed for repository '{{repo_name}}'."

# Update repository dependencies with pnpm
[no-cd]
[group('maintenance')]
update:
    @just {{module_name}}::info "Updating repository '{{repo_name}}'..."
    pnpm update
    @just {{module_name}}::success "Repository '{{repo_name}}' updated successfully."

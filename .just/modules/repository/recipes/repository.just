# Install repository dependencies with pnpm
[no-cd]
install:
    @just {{module_name}}::info "Installing dependencies for repository '{{repo_name}}'..."
    pnpm install
    @just {{module_name}}::success "Dependencies installed for repository '{{repo_name}}'."

# Initialize repository with pnpm, write package.json and add needed dependencies
[no-cd]
init: _init-pnpm && _init-semantic-release update
    @just {{module_name}}::info "Initializing repository '{{repo_name}}'..."

    @just {{module_name}}::info "Adding needed dependencies for repository '{{repo_name}}'..."
    pnpm add -D mega-linter-runner prettier
    @just {{module_name}}::success "Dependencies added for repository '{{repo_name}}'."

    @just {{module_name}}::success "Repository '{{repo_name}}' initialized successfully."

# Update repository dependencies with pnpm
[no-cd]
update:
    @just {{module_name}}::info "Updating repository '{{repo_name}}'..."
    pnpm update
    @just {{module_name}}::success "Repository '{{repo_name}}' updated successfully."

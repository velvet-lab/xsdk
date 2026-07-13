# Install repository dependencies with pnpm
[no-cd]
[group('maintenance')]
install:
    @just {{module_name}}::info "Installing dependencies for repository '{{repo_name}}'..."
    @pnpm install --frozen-lockfile
    @just {{module_name}}::success "Dependencies installed for repository '{{repo_name}}'."

# Update repository dependencies with pnpm
[no-cd]
[group('maintenance')]
update:
    @just {{module_name}}::info "Updating repository '{{repo_name}}'..."
    @pnpm update
    @just {{module_name}}::success "Repository '{{repo_name}}' updated successfully."

# Repariert und bereinigt die lokale Git-Datenbank bei Fehlern (z.B. 'object not found')
[no-cd]
repair-git:
    @just {{module_name}}::info "Cleanup and repair git database..."
    git fsck --full
    git gc --prune=now

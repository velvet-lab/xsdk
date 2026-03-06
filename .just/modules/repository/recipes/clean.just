# Cleanup repository and delete garbage files and folders
[no-cd]
clean: _clean

[private]
[no-cd]
[no-exit-message]
[windows]
_clean:
    @just {{module_name}}::info "Cleaning repository '{{repo_name}}'..."
    -Remove-Item -Path "node_modules" -Recurse -Force -ErrorAction SilentlyContinue
    -Remove-Item -Path "megalinter-reports" -Recurse -Force -ErrorAction SilentlyContinue
    -Remove-Item -Path "TestResults" -Recurse -Force -ErrorAction SilentlyContinue
    -Remove-Item -Path "{{dist_dir}}" -Recurse -Force -ErrorAction SilentlyContinue
    -Remove-Item -Path "packages.lock.json" -Force -ErrorAction SilentlyContinue
    -Remove-Item -Path "pnpm-lock.yaml" -Force -ErrorAction SilentlyContinue
    @just {{module_name}}::success "Repository '{{repo_name}}' cleaned successfully."

[private]
[no-cd]
[no-exit-message]
[unix]
_clean:
    @just {{module_name}}::info "Cleaning repository '{{repo_name}}'..."
    rm -rf "node_modules" || true
    rm -rf "megalinter-reports" || true
    rm -rf "TestResults" || true
    rm -rf "{{dist_dir}}" || true
    rm -f "packages.lock.json" || true
    rm -f "pnpm-lock.yaml" || true
    @just {{module_name}}::success "Repository '{{repo_name}}' cleaned successfully."

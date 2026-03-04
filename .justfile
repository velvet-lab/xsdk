# xSDK - Justfile
# Command runner for development tasks
# See: https://just.systems/man/en/

# Import utilities
import '.just/recipes/common.just'

# List available recipes (default)
default: default-recipe

# Install dependencies for all modules
install:
    bun install

# Clean all artefacts
clean:
    @just info "Cleaning all modules for repo {{repo_name}}..."
    dotnet clean xsdk.sln

    @just info "Removing dist directory '{{dist_dir}}'..."
    -Remove-Item -Path "{{dist_dir}}" -Recurse -Force -ErrorAction SilentlyContinue

    @just info "Removing TestResults directories..."
    -Remove-Item -Path "{{repo_root}}/TestResults" -Recurse -Force -ErrorAction SilentlyContinue

    @just success "Cleaned all modules and removed dist directory."

# Build whole solution
build:
    @just info "Building solution for repo '{{repo_name}}'..."
    dotnet build xsdk.sln --configuration Release
    @just success "Built all modules successfully."

# Tests whole solution
test:
    just info "Running tests for repo '{{repo_name}}'..."
    dotnet test xsdk.sln --configuration Release
    just success "All tests passed successfully."

# Lint whole solution
lint:
    just info "Running linters for repo '{{repo_name}}'..."
    bunx mega-linter-runner --image oxsecurity/megalinter:latest
    just success "Linters completed successfully."

# Lint with auto-fix
lint-fix:
    just info "Running linters with auto-fix for repo '{{repo_name}}'..."
    bunx mega-linter-runner --image oxsecurity/megalinter:latest --fix
    just success "Linters with auto-fix completed successfully."

# Interaktives Changeset mit Bun
change:
    bunx changeset

# Versionen bumpen
bump:
    bunx changeset version

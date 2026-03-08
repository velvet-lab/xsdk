# Creates a GitHub release using Semantic Release, based on commit messages and configured release rules.
[no-cd]
[group('deployment')]
release dry-run:
    @just {{module_name}}::info "Running Semantic Release for repository '{{repo_name}}'..."
    @just {{module_name}}::_release dry-run
    @just {{module_name}}::success "Semantic Release completed for repository '{{repo_name}}'."

[no-cd]
[windows]
_release dry-run:
    if("{{dry-run}}" -eq "true") { pnpm semantic-release --dry-run } else { pnpm semantic-release }

[no-cd]
[linux]
_release dry-run:
    if [ "{{dry-run}}" = "true" ]; then pnpm semantic-release --dry-run; else pnpm semantic-release; fi

[no-cd]
_init-semantic-release:
    @just {{module_name}}::info "Initializing Semantic Release for repository '{{repo_name}}'..."
    pnpm add -D \
        @commitlint/config-conventional \
        @semantic-release/changelog \
        @semantic-release/git \
        @semantic-release/github \
        conventional-changelog-conventionalcommits \
        semantic-release
    @just {{module_name}}::success "Semantic Release initialized for repository '{{repo_name}}'."

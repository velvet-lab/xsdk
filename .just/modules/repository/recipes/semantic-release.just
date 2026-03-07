# Creates a GitHub release using Semantic Release, based on commit messages and configured release rules.
[no-cd]
release:
  pnpm semantic-release

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

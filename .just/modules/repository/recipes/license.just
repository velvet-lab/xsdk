# Validate license headers in all files according to the configuration in .licenserc.yaml
[no-cd]
check-license:
    # @just {{module_name}}::info "Checking license headers in repository '{{repo_name}}'..."
    @pnpm license-check-and-add check; exit 0

# Corrects and updates file headers according to the configuration in .licenserc.yaml
[no-cd]
apply-license:
    @just {{module_name}}::info "Fixing license headers in repository '{{repo_name}}'..."
    @pnpm license-check-and-add add; exit 0

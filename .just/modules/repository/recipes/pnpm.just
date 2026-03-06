
[no-cd]
[windows]
_init-pnpm:
    if(-not (Test-Path -Path "package.json")) { just {{module_name}}::_execute-init-pnpm }

[no-cd]
[linux]
_init-pnpm:
    if [ ! -f package.json ]; then just {{module_name}}::_execute-init-pnpm; fi

[no-cd]
_execute-init-pnpm:
    @just {{module_name}}::info "Initializing pnpm for repository '{{repo_name}}'..."
    pnpm init
    pnpm pkg delete \
        author \
        bugs \
        keywords \
        license \
        main \
        repository \
        scripts
    pnpm pkg set \
        author.name="<Firstname> <Lastname>" \
        author.email="<type@your.mail>" \
        bugs.url="https://github.com/<organisation>/{{repo_name}}/issues" \
        license="MIT" \
        private="true" \
        repository.type="git" \
        repository.url="git+https://github.com/<organisation>/{{repo_name}}.git"
    @just {{module_name}}::success "pnpm initialized for repository '{{repo_name}}'."

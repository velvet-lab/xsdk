
set dotenv-load := true
set shell := ["bash", "-c"]
set windows-shell := ["pwsh.exe", "-NoProfile", "-NoLogo", "-CommandWithArgs"]
set positional-arguments := true
set quiet := true

# Repository-Vars
repo_root := clean(source_directory() / "../..")
repo_name := if os() == 'windows' {
    shell("Split-Path -Path $args[1] -Leaf", repo_root)
} else {
    shell("basename $1", repo_root)
}

dist_dir := repo_root / "dist"

# Private recipe to print colored messages
[private]
[no-quiet]
info message:
    @echo "{{BLUE}}{{message}}{{NORMAL}}"

[private]
[no-quiet]
success message:
    @echo "{{GREEN}}{{message}}{{NORMAL}}"

[private]
[no-quiet]
warning message:
    @echo "{{YELLOW}}{{message}}{{NORMAL}}"

[private]
[no-quiet]
fatal message:
    @echo "{{RED}}{{message}}{{NORMAL}}"
    exit 1

[private]
default-recipe:
    just --list

[private]
init:
    @just info "Initializing repository '{{repo_name}}'..."
    bun init -y
    @just success "Repository '{{repo_name}}' initialized successfully."

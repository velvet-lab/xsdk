
# Repository variables
repo_root := clean(source_directory() / "../../../..")
repo_name := if os() == 'windows' {
    shell("Split-Path -Path $args[1] -Leaf", repo_root)
} else {
    shell("basename $1", repo_root)
}

# Module variables
module_name := "repository"
module_dir := clean(repo_root / ".just/modules" / module_name)

# Dist directory for build outputs
dist_dir := clean(repo_root / "dist")
bin_dir := clean(repo_root / ".bin")

# Cross platform shebang
shebang := if os() == 'windows' {
  'pwsh.exe'
} else {
  '/usr/bin/env pwsh'
}

# Private recipe to print colored messages
[private]
info message:
    @echo "{{BLUE}}{{message}}{{NORMAL}}"

[private]
success message:
    @echo "{{GREEN}}{{message}}{{NORMAL}}"

[private]
warning message:
    @echo "{{YELLOW}}{{message}}{{NORMAL}}"

[private]
fatal message:
    @echo "{{RED}}{{message}}{{NORMAL}}"
    exit 1

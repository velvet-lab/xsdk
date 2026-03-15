# Validate license headers in all files according to the configuration in .licenserc.json
[no-cd]
check-license: _setup-license-tool
    @just {{module_name}}::info "Checking license headers in repository '{{repo_name}}'..."
    {{bin_dir}}/license-eye.exe header check

# Corrects and updates file headers according to the configuration in .licenserc.json
[no-cd]
apply-license: _setup-license-tool
    @just {{module_name}}::info "Fixing license headers in repository '{{repo_name}}'..."
    {{bin_dir}}/license-eye.exe header fix

[no-cd]
[windows]
_setup-license-tool:
    #!{{shebang}}
    $version="0.8.0"
    $url="https://dlcdn.apache.org/skywalking/eyes/$version/skywalking-license-eye-$version-bin.tgz"
    $tarFile = '{{bin_dir}}/eyes.tgz'
    $binFile = '{{bin_dir}}/license-eye.exe'

    New-Item -Path '{{bin_dir}}' -ItemType Directory -ErrorAction Ignore | Out-Null
    if(-not(Test-Path -Path $binFile)) {
        Invoke-WebRequest -Uri $url -OutFile $tarFile
        tar -xzf $tarFile -C {{bin_dir}}
        Move-Item -Path "{{bin_dir}}/skywalking-license-eye-$version-bin/bin/windows/license-eye.exe" -Destination $binFile -Force
        Remove-Item -Path "{{bin_dir}}/skywalking-license-eye-$version-bin" -Recurse -Force
        Remove-Item -Path $tarFile -Force
    }

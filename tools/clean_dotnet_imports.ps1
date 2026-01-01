# Determine Root Folder
$root = $PSScriptRoot
while (-not(Test-Path -Path(Join-Path -Path $root -ChildPath ".git"))) {
    $root = Join-Path -Path $root -ChildPath ".."
}

@(
    "$root"
) | ForEach-Object -Process {
    Get-ChildItem -Path $_ -Filter "obj" -Recurse | Select-Object -ExpandProperty FullName | Remove-Item -Recurse -Force
    Get-ChildItem -Path $_ -Filter "packages.lock.json" -Recurse | Select-Object -ExpandProperty FullName | Remove-Item -Recurse -Force
    Remove-Item -Path "$($_)/dist/*" -Recurse -Force -ErrorAction SilentlyContinue
}


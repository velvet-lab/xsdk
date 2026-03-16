# Publish NuGet packages to registry, requires API key as argument
[no-cd]
[group('deployment')]
publish solution: (build solution)
    #!{{shebang}}
    Remove-Item -Path "{{dist_dir}}/nuget" -Recurse -Force -ErrorAction SilentlyContinue
    dotnet pack "{{solution}}" --configuration RELEASE --nologo --no-build --no-restore --output "{{dist_dir}}/nuget"

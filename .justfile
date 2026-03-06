# xSDK - Justfile
# Command runner for development tasks
# See: https://just.systems/man/en/

# Some settings
set shell := ["bash", "-c"]
set windows-shell := ["pwsh.exe", "-NoProfile", "-NoLogo", "-CommandWithArgs"]
set dotenv-load := true
set quiet := true

# Repository module with recipes for repository management, dependency management and other common tasks
mod repository '.just/modules/repository'

# DotNet module with recipes for building, testing and linting the solution
mod dotnet '.just/modules/dotnet'

[private]
default:
    @just --list

# Initialize repository with pnpm, write package.json and add needed dependencies
[group('maintenance')]
init: repository::init

# Install dependencies for all modules
[group('maintenance')]
install: repository::install  dotnet::install

# Clean whole repository by removing build artifacts, node_modules and other garbage files and folders
[group('maintenance')]
clean:
    @just dotnet::clean xsdk.sln
    @just repository::clean

# Build whole solution
[group('development')]
build: install
    @just dotnet::build xsdk.sln

# Build solution with code coverage for SonarCloud analysis
[group('development')]
sonar-build: install
    @just dotnet::sonar-build xsdk.sln

# Tests whole solution
[group('development')]
test: build
    @just dotnet::test xsdk.sln

# Lint whole solution
[group('development')]
lint:
    @just repository::lint

# Lint with auto-fix
[group('development')]
lint-fix:
    @just repository::lint-fix

release: repository::release

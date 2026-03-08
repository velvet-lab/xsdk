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
init:
    @just repository::init

# Install dependencies for all modules
[group('maintenance')]
install:
    @just repository::install
    @just dotnet::install

# Update dependencies for all modules
[group('maintenance')]
update:
    @just repository::update
    @just dotnet::update

# Clean whole repository by removing build artifacts, node_modules and other garbage files and folders
[group('maintenance')]
clean:
    @just dotnet::clean xsdk.sln
    @just repository::clean

# Build whole solution
[group('development')]
build:
    @just dotnet::build xsdk.sln

# Tests whole solution
[group('development')]
test:
    @just dotnet::test xsdk.sln

# Lint whole solution
[group('linting')]
check-lint:
    @just repository::check-lint
    @just dotnet::check-lint

# Lint with auto-fix
[group('linting')]
lint:
    @just repository::lint
    @just dotnet::lint

# Check code formatting without fixing, useful for CI checks
[group('format')]
check-format:
    @just repository::check-format
    @just dotnet::check-format xsdk-demos.sln

# Format code in whole repository
[group('format')]
format:
    @just repository::format
    @just dotnet::format xsdk-demos.sln

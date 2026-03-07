# Repository - Justfile
# Command runner for development tasks
# See: https://just.systems/man/en/

set shell := ["bash", "-c"]
set windows-shell := ["pwsh.exe", "-NoProfile", "-NoLogo", "-CommandWithArgs"]
set dotenv-load := false
set quiet := true

import 'recipes/module.just'
import 'recipes/repository.just'
import 'recipes/pnpm.just'
import 'recipes/clean.just'
import 'recipes/semantic-release.just'
import 'recipes/lint.just'
import 'recipes/format.just'

# List available recipes
[private]
default:
    @just --list

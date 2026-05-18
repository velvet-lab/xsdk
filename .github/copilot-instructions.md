---
description: "Project-wide Copilot context for xSdk repository"
applyTo: '**/*'
---

Repository: xSdk (velvet-lab/xsdk)

Primary language: C# (.NET 10). Project contains multiple libraries under `libs/`, plugin infrastructure (`xSdk.Plugin`), data providers (EF, MongoDB, Vault, Consul, FlatFile), AI agent integration (`xSdk.Extensions.Agents`), demos, ADRs in `docs/adr/`, and GitHub Actions workflows in `.github/workflows/`.

Guidance for Copilot:
- Prefer repository conventions in `.github/instructions/*`.
- Target .NET 10 and follow `Directory.Build.props` and `global.json`.
- Prioritize secure defaults (no secrets in code); follow `security-and-owasp` guidance.
- For tests, prefer xUnit, keep tests focused and small; aim to improve coverage.
- When changing workflows, pin actions to immutable SHAs and follow least-privilege patterns.

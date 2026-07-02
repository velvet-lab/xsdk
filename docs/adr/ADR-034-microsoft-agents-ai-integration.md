---
title: "ADR-034: Microsoft Agents AI Framework Integration"
status: "Proposed"
date: "2026-07-02"
authors: "GitHub Copilot"
tags: ["architecture", "decision", "ai", "microsoft-agents"]
supersedes: ""
superseded_by: ""
---

## Status

**Proposed**

## Context

Das xSDK-Projekt soll eine Integration mit dem Microsoft Agents AI Framework ermöglichen, um erweiterte KI-Agent-Funktionalitäten bereitzustellen. Dies ermöglicht es Entwicklern, komplexe AI-Agent-Architekturen in ihren Anwendungen zu implementieren, ohne sich um die zugrunde liegende Infrastruktur kümmern zu müssen. Der Microsoft Agents AI Framework bietet Unterstützung für Agent-Hosting, Tool-Integration, Workflow-Management und observability.

Das Projekt hat bereits Erfahrungen mit der Microsoft.Extensions.AI-Bibliothek und der Verwendung von OpenTelemetry-Integration. Wir wollen diesen Ansatz erweitern, um vollständige Integration mit dem Microsoft Agents AI Framework zu ermöglichen.

## Decision

Wir wählen die Integration des Microsoft Agents AI Framework als grundlegende Architekturkomponente zur Unterstützung von KI-Agent-Architekturen in xSDK. Diese Integration wird:

- Die Verwendung von `Microsoft.Agents.AI` durch die `xSdk.Extensions.AI`-Bibliothek ermöglichen
- Einheitliche Schnittstellen zwischen KI-Agent-Funktionalitäten und xSDK-Architektur bereitstellen
- Automatisierte Telemetrie-Integration und Observability in KI-Agent-Anwendungen ermöglichen
- Ein einheitliches Konzept für Tool-Integration in KI-Agenten unterstützen

## Consequences

### Positive

- **POS-001**: Einführung eines modernen, von Microsoft unterstützten AI-Agent-Frameworks
- **POS-002**: Verbesserte Integration und Kompatibilität mit der modernen .NET-Ökosystemlandschaft
- **POS-003**: Vereinheitlichte Implementierung von AI-Agenten über verschiedene Anwendungsbereiche hinweg

### Negative

- **NEG-001**: Abhängigkeit von externen Microsoft-Komponenten, was Updates und Upgrades beeinflussen kann
- **NEG-002**: Erhöhte Komplexität bei der Architektur und Implementierung
- **NEG-003**: Notwendigkeit zusätzlicher Schulung für Entwickler beim Umgang mit dem Microsoft Agents AI Framework

## Alternatives Considered

### Alternative 1: Weiterverwendung von Microsoft.Extensions.AI

- **ALT-001**: **Description**: Fortsetzung der Verwendung der Microsoft.Extensions.AI-Bibliothek mit eigenständiger Implementierung von Agent-Management
- **ALT-001**: **Rejection Reason**: Die Microsoft.Extensions.AI-Bibliothek allein bietet nicht ausreichend Funktionalität für komplexe Agent-Szenarien mit Tool-Integration, Workflows und Host-Managemement

### Alternative 2: Eigenimplementierung des AI-Agent-Frameworks

- **ALT-002**: **Description**: Entwicklung eines eigenständigen, vollständig internen AI-Agent-Frameworks
- **ALT-002**: **Rejection Reason**: Die Entwicklung eines solchen Frameworks würde erhebliche Zeit und Ressourcen erfordern und würde nicht den Vorteilen einer bewährten, von Microsoft untertützten Lösung entsprechen

### Alternative 3: Keine Integration

- **ALT-003**: **Description**: Keine Integration mit einem externen AI-Agent-Framework
- **ALT-003**: **Rejection Reason**: Ein solcher Ansatz würde die Fähigkeit zur Erstellung professioneller AI-Agent-Anwendungen stark einschränken

## Implementation Notes

- **IMP-001**: Anpassung der `xSdk.Extensions.AI`-Bibliothek zur Unterstützung der Microsoft Agents AI Framework-APIs
- **IMP-002**: Integration der Telemetrie-Funktionalität mittels der integrierten OpenTelemetry-Unterstützung von Microsoft Agents
- **IMP-003**: Dokumentation der API-Oberflächen und Beispiele zur Integration in bestehende xSDK-Anwendungen

## References

- **REF-001**: [Microsoft Agents AI Framework Documentation](https://aka.ms/agentsai)
- **REF-002**: [Microsoft.Extensions.AI](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.ai)
- **REF-003**: [ADR-030: AI Agents Extension](./ADR-030-ai-agents-extension.md)

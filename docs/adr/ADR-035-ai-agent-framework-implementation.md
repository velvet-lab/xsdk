---
title: "ADR-035: AI Agent Framework Implementation in xSDK"
status: "Superseded"
date: "2026-07-02"
authors: "GitHub Copilot"
tags: ["architecture", "decision", "ai", "agent-framework"]
supersedes: ""
superseded_by: "ADR-030"
---

## Status

**Superseded by [ADR-030](ADR-030-ai-agents-extension.md)** (2026-07-13)

This ADR proposed an extended AI agent framework. The implementation has been realized in xSdk.Extensions.AI as documented in ADR-030.

## Context

Die xSDK-Bibliothek soll eine vollständige Implementierung eines KI-Agent-Frameworks bieten, das auf den Erkenntnissen aus ADR-030 (AI Agents Extension) basiert. Diese Implementierung wird es Entwicklern ermöglichen, komplexe KI-Agent-Anwendungen in einer konsistenten und skalierbaren Weise zu erstellen, wobei die Integration mit der xSDK-Architektur und den verfügbaren Erweiterungen wie Hosting, Plugins und Telemetrie gewährleistet ist.

Die aktuelle Implementierung von KI-Agenten in xSDK ist auf die Basisfunktionen begrenzt und benötigt eine erweiterte Architektur für komplexe Szenarien, die:

- Tool-Integration und Workflows erfordern
- Auf Plugin-Modularität basieren
- Eine umfangreiche Observability bieten

## Decision

Wir implementieren eine erweiterte Architektur für KI-Agent-Frameworks innerhalb von xSDK, die folgende Elemente umfasst:

1. Einheitliche KI-Agent-Builder-Klassen zur erleichterten Konfiguration
2. Integration von Tool- und Workflow-Management
3. Integration mit Plugin-Systemen für dynamische Agent-Definitionen
4. Standardisierte Telemetrie und Metriken
5. Unterstützung für verschiedene KI-Client-Typen (OpenAI, Azure AI, lokale Modelle)

Diese Lösung wird die `xSdk.Extensions.AI`-Bibliothek erweitern und auf das Microsoft Agents AI-Framework aufbauen.

## Consequences

### Positive

- **POS-001**: Strukturierte und einheitliche Implementierung von KI-Agent-Anwendungen
- **POS-002**: Verbesserte Wiederverwendbarkeit und Modularität von KI-Agenten
- **POS-003**: Nahtlose Integration in die vorhandene xSDK-Architektur und -Erweiterungen

### Negative

- **NEG-001**: Erhöhte Komplexität in der Implementierung und Wartung der KI-Agent-Framework-Komponenten
- **NEG-002**: Möglicher erhöhter Performance-Overhead bei der Agent-Initialisierung
- **NEG-003**: Notwendigkeit zusätzlicher Schulung für Entwickler hinsichtlich des KI-Agent-Frameworks

## Alternatives Considered

### Alternative 1: Weiterverwendung der grundlegenden Implementierung

- **ALT-001**: **Description**: Weiterverwendung der in ADR-030 beschriebenen grundlegenden Implementierung
- **ALT-001**: **Rejection Reason**: Die grundlegende Implementierung deckt komplexe Szenarien mit Tool-Integration, Workflows und dynamischem Plugin-Management nicht aus

### Alternative 2: Eigenständiges Framework außerhalb von xSDK

- **ALT-002**: **Description**: Entwicklung eines separaten, eigenständigen KI-Agent-Frameworks
- **ALT-002**: **Rejection Reason**: Eine Separate Implementierung würde die Integration mit xSDK-Ressourcen wie Hosting, Konfiguration und Telemetrie erschweren und Duplikation von Funktionalitäten verursachen

### Alternative 3: Integration eines Drittanbieter-Frameworks

- **ALT-003**: **Description**: Nutzung eines externen KI-Agent-Frameworks statt eines internen
- **ALT-003**: **Rejection Reason**: Externe Frameworks bieten geringere Kontrolle, weniger Anpassungsmöglichkeiten und könnten die Integration in das xSDK-Ökosystem beeinträchtigen

## Implementation Notes

- **IMP-001**: Entwicklung des `AILayerBuilder`-Systems für komplexe KI-Agent-Konfigurationen
- **IMP-002**: Implementierung von `AIPluginBuilder` zur dynamischen Plugin-Definition für Agenten
- **IMP-003**: Integration von Workflows für fortgeschrittene Agent-Abläufe
- **IMP-004**: Erstellung von Vorlagen und Demonstrationsbeispielen für komplexe Agenten

## References

- **REF-001**: [ADR-030: AI Agents Extension](./ADR-030-ai-agents-extension.md)
- **REF-002**: [ADR-034: Microsoft Agents AI Framework Integration](./ADR-034-microsoft-agents-ai-integration.md)
- **REF-003**: [Microsoft Agents AI Documentation](https://aka.ms/agentsai)

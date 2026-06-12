---
title: "ADR-033: Logging Architecture with QueueLogger"
status: "Accepted"
date: "2026-06-12"
authors: "Roland Breitschaft"
tags: ["architecture", "decision", "logging"]
supersedes: ""
superseded_by: ""
---

## Status

Accepted

## Context

The xSdk project faced a challenge with logging configuration in a two-phase hosting pattern (SlimHost → Host). Initially, both SlimHost and Host were creating separate `ILoggerFactory` instances, causing:

1. **Dual LoggerFactories**: Two separate logging factories were created, leading to configuration loss
2. **Configuration Loss**: Plugin logging configurations were not properly applied to all loggers
3. **Inconsistent Logging**: Different logging behavior between SlimHost and Host phases
4. **Plugin Filtering Issues**: Unable to filter log output per plugin as required

The core problem was that:
- SlimHost needed to support DI for plugins (which require `ILogger<T>`)
- Host needed to apply full plugin configurations
- Both phases needed to share the same logging infrastructure
- Early logging (before Host start) needed to be preserved

## Decision

We implemented a new logging architecture using:
1. **QueueLogger**: A custom `ILogger` implementation that buffers log messages
2. **QueueLoggerProvider**: Creates QueueLogger instances for DI
3. **LogManager**: Centralized logging manager that coordinates between phases
4. **Two-Phase Registration**: 
   - SlimHost registers QueueLoggerProvider for DI
   - Host initializes real logging factory and flushes queue

## Consequences

### Positive

- **Single Source of Truth**: One logging infrastructure shared between SlimHost and Host
- **Plugin Filtering**: Each plugin can now have its own logging configuration
- **No Configuration Loss**: All log messages are preserved and properly routed
- **Clean Separation**: SlimHost handles buffering, Host handles real logging
- **Backward Compatibility**: Existing code continues to work unchanged

### Negative

- **Complexity**: Additional components (QueueLogger, QueueLoggerProvider)
- **Performance Overhead**: Small overhead for message buffering
- **Memory Usage**: Temporary memory usage for buffered messages
- **Increased Cognitive Load**: More components to understand and maintain

### Implementation Notes

- **SlimHost Phase**: Uses QueueLoggerProvider for DI, buffers messages
- **Host Phase**: Initializes real ILoggerFactory, flushes QueueLogger buffer
- **Dependency Injection**: Works seamlessly with existing DI patterns
- **Plugin Support**: Full plugin logging configuration support

## Alternatives Considered

### Alternative 1: Factory Upgrade Pattern
- **Pros**: Single factory instance
- **Cons**: Complex upgrade logic, disposal management, potential race conditions

### Alternative 2: Shared Factory with Callbacks
- **Pros**: Centralized control
- **Cons**: Complex callback mechanism, tight coupling between phases

### Alternative 3: Global Logger Instance
- **Pros**: Simple implementation
- **Cons**: No plugin filtering, difficult to manage lifecycle

### Alternative 4: Separate Logging for Each Phase
- **Pros**: Simple separation
- **Cons**: Inconsistent logging experience, configuration loss

## References

- [ADR-013: NLog Logging Framework](ADR-013-nlog-logging-framework.md)
- [ADR-026: SlimHost Builder Redesign](ADR-026-slim-host-builder-redesign.md)
- [ADR-027: Plugin Host Model](ADR-027-plugin-host-model.md)

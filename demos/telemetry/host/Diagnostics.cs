using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace xSdk;

/// <summary>
/// Diagnostics-Klasse für das Telemetry-Demo (ADR-014).
/// Explizite Compile-Zeit-Konstante — kein Reflection, kein lautloser Bruch bei Projekt-Rename.
/// </summary>
internal static class Diagnostics
{
    /// <summary>Name der ActivitySource und des Meters — manuell gesetzt, entspricht der fachlichen Identität.</summary>
    internal const string SourceName = "xSdk.Demos.Telemetry";

    /// <summary>Gemeinsame <see cref="ActivitySource"/> für alle Klassen im Telemetry-Demo.</summary>
    internal static readonly ActivitySource Source = new(SourceName);

    /// <summary>Gemeinsamer <see cref="Meter"/> für alle Klassen im Telemetry-Demo.</summary>
    internal static readonly Meter Meter = new(SourceName);
}

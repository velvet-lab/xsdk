/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace xSdk.Extensions.AI;

/// <summary>
/// Zentrale Diagnostics-Klasse für das Paket <c>xSdk.Extensions.AI</c> (ADR-014).
/// Der Source-Name ist eine Compile-Zeit-Konstante — keine Reflection, kein lautloser Bruch bei Assembly-Rename.
/// </summary>
internal static class Diagnostics
{
    /// <summary>Name der ActivitySource und des Meters — entspricht dem Assembly-Namen.</summary>
    internal const string SourceName = "xSdk.Extensions.AI";

    /// <summary>Gemeinsame <see cref="ActivitySource"/> für alle Klassen in <c>xSdk.Extensions.AI</c>.</summary>
    internal static readonly ActivitySource Source = new(SourceName);

    /// <summary>Gemeinsamer <see cref="Meter"/> für alle Klassen in <c>xSdk.Extensions.AI</c>.</summary>
    internal static readonly Meter Meter = new(SourceName);
}

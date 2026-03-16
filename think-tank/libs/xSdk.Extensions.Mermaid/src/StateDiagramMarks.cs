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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Extensions.Mermaid
{
    internal class StateDiagramMarks
    {
        internal const string Diagram = "stateDiagram";
        internal const string Transition = "-->";
        internal const string Value = ":";
        internal const string Inital = "[*]";
        internal const string Final = Inital;
        internal const string State = "state";
        internal const string OpenBracket = "{";
        internal const string CloseBracket = "}";
        internal const string Comment = "%%";
        internal const string Choice = "<<choice>>";
        internal const string Fork = "<<fork>>";
        internal const string Join = "<<join>>";
    }
}

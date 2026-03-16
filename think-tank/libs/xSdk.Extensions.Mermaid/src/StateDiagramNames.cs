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

namespace xSdk.Extensions.Mermaid
{
    internal static class StateDiagramNames
    {
        internal const string DiagramType = nameof(DiagramType);
        internal const string Title = nameof(Title);
        internal const string Version = nameof(Version);

        internal const string Initial = nameof(Initial);
        internal const string Final = nameof(Final);
        internal const string Transition = nameof(Transition);

        // internal const string TransitionWithDescription = nameof(TransitionWithDescription);
        internal const string BlockBegin = nameof(BlockBegin);
        internal const string BlockEnd = nameof(BlockEnd);
        internal const string Concurrency = nameof(Concurrency);
        internal const string FrontMatter = nameof(FrontMatter);
        internal const string DescriptionLong = nameof(DescriptionLong);
        internal const string DescriptionShort = nameof(DescriptionShort);
        internal const string Choice = nameof(Choice);
        internal const string Fork = nameof(Fork);
        internal const string Join = nameof(Join);

        //internal const string NoteBegin = nameof(NoteBegin);
        //internal const string NoteEnd = nameof(NoteEnd);
        internal const string Direction = nameof(Direction);
        internal const string Comment = nameof(Comment);
        internal const string SequenceTerminator = nameof(SequenceTerminator);
    }
}

using System.Collections.Generic;

namespace xSdk.Extensions.Mermaid.Lexer
{
    internal sealed class StateDiagramTokenizer : TokenizerBase
    {
        protected override IEnumerable<TokenDefinition> TokenDefinitions =>
            new List<TokenDefinition>()
            {
                new TokenDefinition(StateDiagramNames.DiagramType, StateDiagramMarks.Diagram),
                new TokenDefinition(StateDiagramNames.DescriptionShort, @"\w.+:.+", 4),
                new TokenDefinition(StateDiagramNames.Transition, @"\w.+-->.+", 3),
                new TokenDefinition(StateDiagramNames.Final, @"\w.+-->.\[\*\]", 2),
                new TokenDefinition(StateDiagramNames.Initial, @"\[\*\].-->.+"),
                new TokenDefinition(StateDiagramNames.BlockBegin, @"state.+{"),
                new TokenDefinition(StateDiagramNames.BlockEnd, @"}"),
                new TokenDefinition(StateDiagramNames.FrontMatter, @"---"),
                new TokenDefinition(StateDiagramNames.Concurrency, @"--"),
                new TokenDefinition(StateDiagramNames.DescriptionLong, @"state.+"".+"".+as.+"),
                new TokenDefinition(StateDiagramNames.Choice, @"state.+<<choice>>"),
                new TokenDefinition(StateDiagramNames.Fork, @"state.+<<fork>>"),
                new TokenDefinition(StateDiagramNames.Join, @"state.+<<join>>"),
                new TokenDefinition(StateDiagramNames.Title, @"title.+"),
                new TokenDefinition(StateDiagramNames.Version, @"version.+"),
                //new TokenDefinition(StateDiagramNames.NoteBegin, @"note.+"),
                //new TokenDefinition(StateDiagramNames.NoteEnd, @"end note"),
                new TokenDefinition(StateDiagramNames.Direction, @"direction LR"),
                new TokenDefinition(StateDiagramNames.Comment, @"%%.+"),
            };

        protected override TokenDefinition Terminator => new TokenDefinition(StateDiagramNames.SequenceTerminator);
    }
}

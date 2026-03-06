using System;
using System.Collections.Generic;
using xSdk.Extensions.Mermaid.Data;
using xSdk.Extensions.Mermaid.Lexer;

namespace xSdk.Extensions.Mermaid.Parser
{
    internal sealed partial class StateDiagramParser
    {
        private ICollection<Transition> Transitions;

        private ICollection<Composition> Compositions;

        private ICollection<Choice> Choices;

        private ICollection<Fork> Forks;

        private ICollection<Join> Joins;

        private ICollection<Description> Descriptions;

        private string Title;

        private string Version;

        private string _currentComment;

        private void ParseInternal(IEnumerable<Token> tokens)
        {
            var isHeaderRegion = true;
            Composition currentComposition = null;

            foreach (var token in tokens)
            {
                if (!isHeaderRegion)
                {
                    if (token.Definition.Name == StateDiagramNames.Comment)
                    {
                        ParseComment(token);
                    }

                    if (token.Definition.Name == StateDiagramNames.BlockBegin)
                    {
                        var composition = ParseComposition(token);
                        if (currentComposition != null)
                        {
                            composition.Parent = currentComposition;
                            currentComposition.Compositions.Add(composition);
                        }
                        else
                        {
                            Compositions.Add(composition);
                        }
                        currentComposition = composition;
                    }

                    if (token.Definition.Name == StateDiagramNames.BlockEnd)
                    {
                        if (currentComposition != null && currentComposition.Parent != null)
                        {
                            currentComposition = currentComposition.Parent;
                        }
                        else
                        {
                            currentComposition = null;
                        }
                    }

                    if (token.Definition.Name == StateDiagramNames.Choice)
                    {
                        Choices.Add(ParseChoice(token));
                    }

                    if (token.Definition.Name == StateDiagramNames.Fork)
                    {
                        Forks.Add(ParseFork(token));
                    }

                    if (token.Definition.Name == StateDiagramNames.Join)
                    {
                        Joins.Add(ParseJoin(token));
                    }

                    if (
                        token.Definition.Name == StateDiagramNames.Initial
                        || token.Definition.Name == StateDiagramNames.Final
                        || token.Definition.Name == StateDiagramNames.Transition
                    )
                    {
                        var transition = ParseTransition(token);

                        if (currentComposition != null)
                        {
                            currentComposition.Transitions.Add(transition);
                        }
                        else
                        {
                            Transitions.Add(transition);
                        }
                    }

                    if (token.Definition.Name == StateDiagramNames.DescriptionShort)
                    {
                        Descriptions.Add(ParseShortDescription(token));
                    }

                    if (token.Definition.Name == StateDiagramNames.DescriptionLong)
                    {
                        Descriptions.Add(ParseLongDescription(token));
                    }
                }
                else
                {
                    if (token.Definition.Name == StateDiagramNames.Title)
                    {
                        Title = ParseHeaderItem(token);
                    }

                    if (token.Definition.Name == StateDiagramNames.Version)
                    {
                        Version = ParseHeaderItem(token);
                    }
                }

                if (token.Definition.Name == StateDiagramNames.DiagramType)
                {
                    isHeaderRegion = false;
                }
            }
        }

        private Transition ParseTransition(Token token)
        {
            var splitted = token.Value?.Split(StateDiagramMarks.Transition, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var sourceState = ParseState(splitted[0], true);
            var targetStateValue = splitted[1];

            splitted = targetStateValue.Split(StateDiagramMarks.Value, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            var targetState = ParseState(splitted[0], false);

            var transition = new Transition
            {
                Source = sourceState,
                Target = targetState,
                Value = splitted.Length > 1 ? splitted[1] : null,
            };

            SetComment(transition);

            return transition;
        }

        private Composition ParseComposition(Token token)
        {
            var value = token.Value?.Replace(StateDiagramMarks.State, "");
            value = value?.Replace("{", "").Trim();

            var composition = new Composition { Name = value };

            SetComment(composition);

            return composition;
        }

        private State ParseState(string value, bool isSourceState)
        {
            var state = new State
            {
                Name = value,
                IsInitial = string.Compare(value, StateDiagramMarks.Inital, true) == 0 && isSourceState ? true : false,
                IsFinal = string.Compare(value, StateDiagramMarks.Final, true) == 0 && !isSourceState ? true : false,
            };

            return state;
        }

        private Choice ParseChoice(Token token)
        {
            var value = token.Value?.Substring(StateDiagramMarks.State.Length);
            value = value?.Replace(StateDiagramMarks.Choice, "");
            var choice = new Choice { Name = value.Trim() };

            SetComment(choice);

            return choice;
        }

        private Fork ParseFork(Token token)
        {
            var value = token.Value?.Substring(StateDiagramMarks.State.Length);
            value = value?.Replace(StateDiagramMarks.Fork, "");

            var fork = new Fork { Name = value.Trim() };

            SetComment(fork);

            return fork;
        }

        private Join ParseJoin(Token token)
        {
            var value = token.Value?.Substring(StateDiagramMarks.State.Length);
            value = value?.Replace(StateDiagramMarks.Join, "");

            var join = new Join { Name = value.Trim() };

            SetComment(join);

            return join;
        }

        private void ParseComment(Token token)
        {
            _currentComment = token.Value?.Replace(StateDiagramMarks.Comment, "").Trim();
        }

        private Description ParseShortDescription(Token token)
        {
            var splitted = token.Value?.Split(StateDiagramMarks.Value, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var description = new Description { Name = splitted[0], Text = splitted[1] };

            SetComment(description);

            return description;
        }

        private Description ParseLongDescription(Token token)
        {
            var value = token.Value?.Replace(StateDiagramMarks.State, "");
            var splitted = value?.Split("as", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            var description = new Description { Name = splitted[1], Text = splitted[0] };

            SetComment(description);

            return description;
        }

        private void SetComment<TObject>(TObject item)
            where TObject : ISupportComments
        {
            if (!string.IsNullOrEmpty(_currentComment))
            {
                item.Comment = _currentComment;
                _currentComment = null;
            }
        }

        private string ParseHeaderItem(Token token) =>
            token.Value.Split(StateDiagramMarks.Value, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1];
    }
}

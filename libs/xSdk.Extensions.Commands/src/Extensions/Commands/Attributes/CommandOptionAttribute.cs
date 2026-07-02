using System;
using System.Collections.Generic;
using System.Text;

namespace xSdk.Extensions.Commands.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
internal class CommandOptionAttribute : Attribute
{
    public string Name { get; }

    public string[] Aliases { get; }

    public CommandOptionAttribute(string name, params string[] aliases)
    {
        Name = name;
        Aliases = aliases;
    }
}

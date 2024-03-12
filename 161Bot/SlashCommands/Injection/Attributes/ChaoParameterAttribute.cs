using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace _161Bot.SlashCommands.Injection.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class ChaoParameterAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public ChaoParameterAttribute(string name, string description, bool required = true)
        {
            this.Name = name;
            this.Description = description;
            this.Required = required;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace _161Bot.SlashCommands.Injection.Attributes
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ChaoCommandAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ChaoCommandAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}

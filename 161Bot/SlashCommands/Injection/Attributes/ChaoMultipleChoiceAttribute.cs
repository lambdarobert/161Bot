using System;
using System.Collections.Generic;
using System.Text;

namespace _161Bot.SlashCommands.Injection.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public class ChaoMultipleChoiceAttribute : Attribute
    {
        //a list of multiple choices for a slash command, in the format key=value to split and send off to Discord.
        public string[] Choices { get; set; }
        public ChaoMultipleChoiceAttribute(params string[] choices)
        {
            this.Choices = choices;
        }
    }
}

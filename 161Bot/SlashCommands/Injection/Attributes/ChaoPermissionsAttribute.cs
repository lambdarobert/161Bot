using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace _161Bot.SlashCommands.Injection.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ChaoPermissionsAttribute : Attribute
    {
        public IList<ApplicationCommandPermission> Perms { get; set; }
        public ChaoPermissionsAttribute(ulong[] userids, params ulong[] roleids)
        {
            Perms = new List<ApplicationCommandPermission>();
            foreach(var id in userids)
            {
                Perms.Add(new ApplicationCommandPermission(id, ApplicationCommandPermissionTarget.User, true));
            }
            foreach(var id in roleids)
            {
                Perms.Add(new ApplicationCommandPermission(id, ApplicationCommandPermissionTarget.Role, true));
            }
        }
    }
}

using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace _161Bot
{
    /*
     * This class makes it easier to specify an audit log reason by not having to do this every time.
     */
    public class AuditLogReason : RequestOptions
    {
        public AuditLogReason(string reason) : base()
        {
            this.AuditLogReason = reason;
        }
    }
}

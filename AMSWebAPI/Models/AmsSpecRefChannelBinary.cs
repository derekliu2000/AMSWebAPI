using System;
using System.Collections.Generic;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AmsSpecRefChannelBinary
    {
        public DateTime Utc { get; set; }
        public byte CId { get; set; }
        public short Header { get; set; }
        public byte[] SpecRef { get; set; }

        public virtual AmsSpecRefBinary UtcNavigation { get; set; }
    }
}

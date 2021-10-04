using System;
using System.Collections.Generic;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AmsSpecRefBinary
    {
        public AmsSpecRefBinary()
        {
            AmsSpecRefChannelBinaries = new HashSet<AmsSpecRefChannelBinary>();
        }

        public DateTime Utc { get; set; }
        public byte[] SpecRef { get; set; }

        public virtual ICollection<AmsSpecRefChannelBinary> AmsSpecRefChannelBinaries { get; set; }
    }
}

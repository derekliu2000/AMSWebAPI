using System;
using System.Collections.Generic;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AmsSetting
    {
        public int Id { get; set; }
        public byte[] Settings { get; set; }
        public DateTime LastUpdateUtc { get; set; }
    }
}

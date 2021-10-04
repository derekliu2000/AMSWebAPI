using System;
using System.Collections.Generic;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AmsSettingsString
    {
        public int Id { get; set; }
        public string Settings { get; set; }
        public DateTime LastUpdateUtc { get; set; }
    }
}

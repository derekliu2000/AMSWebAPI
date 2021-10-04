using System;
using System.Collections.Generic;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AmsFileLastUpdateUtc
    {
        public string FileName { get; set; }
        public DateTime LastModifiedUtc { get; set; }
    }
}

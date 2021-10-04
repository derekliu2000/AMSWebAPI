using System;
using System.Collections.Generic;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AmsSensorLocation
    {
        public DateTime Utc { get; set; }
        public short SensorId { get; set; }
        public short XLoc { get; set; }
        public short YLoc { get; set; }
        public string Label { get; set; }
        public short Type { get; set; }
        public bool IsInFront { get; set; }

        public virtual AmsSensorLocBinary UtcNavigation { get; set; }
    }
}

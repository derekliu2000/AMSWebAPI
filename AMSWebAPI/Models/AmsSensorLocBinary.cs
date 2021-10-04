using System;
using System.Collections.Generic;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AmsSensorLocBinary
    {
        public AmsSensorLocBinary()
        {
            AmsSensorLocations = new HashSet<AmsSensorLocation>();
        }

        public DateTime Utc { get; set; }
        public byte[] SensorLoc { get; set; }

        public virtual ICollection<AmsSensorLocation> AmsSensorLocations { get; set; }
    }
}

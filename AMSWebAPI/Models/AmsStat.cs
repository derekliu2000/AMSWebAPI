using System;
using System.Collections.Generic;

#nullable disable

namespace AMSWebAPI.Models
{
    public partial class AmsStat
    {
        public short Afbidx { get; set; }
        public short Afbjid { get; set; }
        public short ChannelIdx { get; set; }
        public short? CapRef { get; set; }
        public short? CapMeasured { get; set; }
        public short? Afbtemp { get; set; }
    }
}

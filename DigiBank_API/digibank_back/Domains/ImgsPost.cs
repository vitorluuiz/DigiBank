using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class ImgsPost
    {
        public short IdImg { get; set; }
        public byte? IdPost { get; set; }
        public string Img { get; set; }

        public virtual Marketplace IdPostNavigation { get; set; }
    }
}

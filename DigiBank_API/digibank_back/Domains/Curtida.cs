using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Curtida
    {
        public int IdCurtida { get; set; }
        public int IdAvaliacao { get; set; }
        public int IdUsuario { get; set; }

        public virtual Avaliaco IdAvaliacaoNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}

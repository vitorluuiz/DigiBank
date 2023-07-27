using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Avaliaco
    {
        public Avaliaco()
        {
            Curtida = new HashSet<Curtida>();
        }

        public int IdAvaliacao { get; set; }
        public int IdUsuario { get; set; }
        public byte IdPost { get; set; }
        public decimal Nota { get; set; }
        public short Replies { get; set; }
        public DateTime DataPostagem { get; set; }
        public string Comentario { get; set; }

        public virtual Marketplace IdPostNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Curtida> Curtida { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Condico
    {
        public Condico()
        {
            Emprestimos = new HashSet<Emprestimo>();
        }

        public byte IdCondicao { get; set; }
        public string Condicao { get; set; }

        public virtual ICollection<Emprestimo> Emprestimos { get; set; }
    }
}

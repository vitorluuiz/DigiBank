using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace digibank_back.Domains
{
    public partial class AcoesOption
    {
        public AcoesOption()
        {
            Acos = new HashSet<Aco>();
        }

        public byte IdAcaoOption { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public decimal Dividendos { get; set; }

        [Required(ErrorMessage = "É preciso informar a quantidade de cotas disponíveis")]
        public short CotasDisponiveis { get; set; }
        public string AcaoImg { get; set; }

        public virtual ICollection<Aco> Acos { get; set; }
    }
}

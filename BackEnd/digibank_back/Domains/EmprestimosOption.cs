using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class EmprestimosOption
    {
        public EmprestimosOption()
        {
            Emprestimos = new HashSet<Emprestimo>();
        }

        public byte IdEmprestimoOption { get; set; }
        public decimal Valor { get; set; }
        public decimal TaxaJuros { get; set; }
        public decimal RendaMinima { get; set; }

        public virtual ICollection<Emprestimo> Emprestimos { get; set; }
    }
}

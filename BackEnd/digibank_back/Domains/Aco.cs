using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Aco
    {
        
        public short IdAcao { get; set; }
        public short? IdUsuario { get; set; }
        public byte? IdAcoesOptions { get; set; }

        [Required(ErrorMessage = "É Necessário informar a quantidade de cotas")]
        public byte QntCotas { get; set; }

        [Required(ErrorMessage = "É necessário informar a data de aquisição")]
        public DateTime DataAquisicao { get; set; }

        [Required(ErrorMessage = "É necessário informar o valor inicial")]
        public decimal ValorInicial { get; set; }

        public virtual AcoesOption IdAcoesOptionsNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Transaco
    {
        public short IdTransacao { get; set; }

        [Required(ErrorMessage = "Usuário pagante obrigatório")]
        public short? IdUsuarioPagante { get; set; }

        [Required(ErrorMessage = "Usuário recebente necessário")]
        public short? IdUsuarioRecebente { get; set; }

        [Required(ErrorMessage = "Valor da transação precisa ser informado")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Data da transação necessária")]
        public DateTime? DataTransacao { get; set; }
        public string Descricao { get; set; }

        public virtual Usuario IdUsuarioPaganteNavigation { get; set; }
        public virtual Usuario IdUsuarioRecebenteNavigation { get; set; }
    }
}

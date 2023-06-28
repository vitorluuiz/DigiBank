using System;
using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel
{
    public class MarketPlaceViewModel
    {
        [Required(ErrorMessage = "É necessário associar um Usuário ao Post")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "É preciso informar um título ao Post")]
        [MaxLength(40)]
        public string Titulo { get; set; }

        [MaxLength(200)]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "É preciso informar um valor, mesmo que seja 0 (zero)")]
        [Range(0, 9999999.99, ErrorMessage = "O valor da postagem deve estar entre 0 e 9.999.999,99")]
        public decimal Valor { get; set; }

        [MaxLength(6)]
        public string MainColorHex { get; set; }
        public bool IsVirtual { get; set; }
    }
}

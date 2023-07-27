using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.DTOs
{
    public class PostGenerico
    {
        public int IdPost { get; set; }
        public int IdUsuario { get; set; }
        public string ApelidoProprietario { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string MainImg { get; set; }
        public string MainColorHex { get; set; }
        public decimal Valor { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsActive { get; set; }
        public short? Vendas { get; set; }
        public short QntAvaliacoes { get; set; }
        public decimal Avaliacao { get; set; }
        public List<string> Imgs { get; set; }

        public PostGenerico(Marketplace p, List<string> imgs)
        {
            IdPost = p.IdPost;
            IdUsuario = p.IdUsuario;
            ApelidoProprietario = p.IdUsuarioNavigation?.Apelido;
            Nome = p.Nome;
            Descricao = p.Descricao;
            MainImg = p.MainImg;
            MainColorHex = p.MainColorHex;
            Valor = p.Valor;
            IsVirtual = p.IsVirtual;
            IsActive = p.IsActive;
            Vendas = p.Vendas;
            QntAvaliacoes = (short)p.QntAvaliacoes;
            Avaliacao = (decimal)p.Avaliacao;
            Imgs = imgs;
        }
    }
}

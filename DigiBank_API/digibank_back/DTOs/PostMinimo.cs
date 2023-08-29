using digibank_back.Domains;

namespace digibank_back.DTOs
{
    public class PostMinimo
    {
        public int IdPost { get; set; }
        public string ApelidoProprietario { get; set; }
        public string Nome { get; set; }
        public string MainImg { get; set; }
        public string MainColorHex { get; set; }
        public decimal Valor { get; set; }
        public decimal Avaliacao { get; set; }
        public short? Vendas { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsActive { get; set; }

        public PostMinimo(Marketplace p)
        {
            IdPost = p.IdPost;
            ApelidoProprietario = p.IdUsuarioNavigation.Apelido;
            Nome = p.Nome;
            MainImg = p.MainImg;
            MainColorHex = p.MainColorHex;
            Valor = p.Valor;
            Avaliacao = (short)p.Avaliacao;
            Vendas = p.Vendas;
            IsVirtual= p.IsVirtual;
            IsActive = p.IsActive;
        }
    }
}

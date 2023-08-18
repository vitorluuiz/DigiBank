using digibank_back.Domains;

namespace digibank_back.DTOs
{
    public class PostTitle
    {
        public int IdPost { get; set; }
        public string Titulo { get; set; }
        public decimal Valor { get; set; }
        public string MainImg { get; set; }
        public string MainColorHex { get; set; }

        public PostTitle(Marketplace p)
        {
            IdPost = p.IdPost;
            Titulo = p.Nome;
            Valor = p.Valor;
            MainImg = p.MainImg;
            MainColorHex = p.MainColorHex;
        }
    }
}

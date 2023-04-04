namespace digibank_back.DTOs
{
    public class PostGenerico
    {
        public int IdPost { get; set; }
        public int Idusuario { get; set; }
        public string ApelidoProprietario { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string MainImg { get; set; }
        public decimal Valor { get; set; }
        public bool IsVirtual { get; set; }
        public short Vendas { get; set;}
        public short QntAvaliacoes { get; set; }
        public decimal Avaliacao { get; set; }
    }
}

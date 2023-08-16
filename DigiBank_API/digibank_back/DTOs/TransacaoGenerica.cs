using digibank_back.Domains;
using System;

namespace digibank_back.DTOs
{
    public class TransacaoGenerica
    {
        public int IdTransacao { get; set; }
        public int IdUsuarioPagante { get; set; }
        public string NomePagante { get; set; }
        public int IdUsuarioRecebente { get; set; }
        public string NomeRecebente { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataTransacao { get; set; }
        public string Descricao { get; set; }

        public TransacaoGenerica(Transaco t)
        {
            IdTransacao = t.IdTransacao;
            IdUsuarioPagante = t.IdUsuarioPagante;
            NomePagante = t.IdUsuarioPaganteNavigation.NomeCompleto;
            IdUsuarioRecebente = t.IdUsuarioRecebente;
            NomeRecebente = t.IdUsuarioRecebenteNavigation.NomeCompleto;
            Valor = t.Valor;
            DataTransacao = t.DataTransacao;
            Descricao = t.Descricao;
        }
    }
}

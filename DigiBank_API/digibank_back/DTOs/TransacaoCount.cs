using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.DTOs
{
    public class TransacaoCount
    {
        public int TransacoesCount { get; set; }

        public List<Transaco> Transacoes { get; set; }
    }
}

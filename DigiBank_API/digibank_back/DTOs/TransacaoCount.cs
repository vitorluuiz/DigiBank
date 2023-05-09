using digibank_back.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace digibank_back.DTOs
{
    public class TransacaoCount
    {
        public int TransacoesCount { get; set; }

        public List<Transaco> Transacoes { get; set; }
    }
}

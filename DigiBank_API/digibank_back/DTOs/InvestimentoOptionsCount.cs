using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace digibank_back.DTOs
{
    public class InvestimentoOptionsCount
    {
        public int OptionsCount { get; set; }

        public List<InvestimentoOptionGenerico> InvestimentosOptions { get; set; }
    }
}

using System.Collections.Generic;

namespace digibank_back.DTOs
{
    public class EmblemaInvestOption
    {
        public int IdEmblema { get; set; }
        public string Emblema { get; set; }
        public double Corte { get; set; }

        public List<EmblemaInvestOption> GetEmblemas()
        {
            return new List<EmblemaInvestOption>{
                new EmblemaInvestOption
                {
                    IdEmblema = 1,
                    Emblema = "Maior Valorização",
                    Corte = 0.20
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 2,
                    Emblema = "Maiores dividendos",
                    Corte = 0.20
                }
            };
        }
    }
}

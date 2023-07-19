using System.Collections.Generic;

namespace digibank_back.DTOs
{
    public class EmblemaInvestOption
    {
        public int IdEmblema { get; set; }
        public string Emblema { get; set; }
        public int Valor { get; set; }
        public int Tipo { get; set; }
        public double Corte { get; set; }

        public List<EmblemaInvestOption> GetEmblemas()
        {
            return new List<EmblemaInvestOption>{
                new EmblemaInvestOption
                {
                    IdEmblema = 1,
                    Emblema = "Maiores dividendos",
                    Valor = 1,
                    Tipo = 1,
                    Corte = 0.10
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 2,
                    Emblema = "Dividendos altos",
                    Valor = 2,
                    Tipo = 1,
                    Corte = 0.20
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 3,
                    Emblema = "Dividendos acima da média",
                    Valor = 3,
                    Tipo = 1,
                    Corte = 0.30
                },new EmblemaInvestOption
                {
                    IdEmblema = 4,
                    Emblema = "Cotas mais baratas",
                    Valor = 1,
                    Tipo = 2,
                    Corte = 0.10
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 5,
                    Emblema = "Cotas acessíveis",
                    Valor = 2,
                    Tipo = 2,
                    Corte = 0.20
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 5,
                    Emblema = "Valores iniciais",
                    Valor = 3,
                    Tipo = 2,
                    Corte = 0.50
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 5,
                    Emblema = "Gigante mundial",
                    Valor = 1,
                    Tipo = 3,
                    Corte = 0.08
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 5,
                    Emblema = "Grande marca",
                    Valor = 2,
                    Tipo = 3,
                    Corte = 0.15
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 5,
                    Emblema = "Destaque nacional",
                    Valor = 3,
                    Tipo = 3,
                    Corte = 0.22
                },
                new EmblemaInvestOption
                {
                    IdEmblema = 5,
                    Emblema = "Destaque regional",
                    Valor = 4,
                    Tipo = 3,
                    Corte = 0.30
                },
            };
        }
    }
}

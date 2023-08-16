using digibank_back.Domains;
using System;

namespace digibank_back.DTOs
{
    public class InvestimentoOptionGenerico
    {
        public short IdInvestimentoOption { get; set; }
        public byte IdTipo { get; set; }
        public short IdArea { get; set; }
        public string Tipo { get; set; }
        public string Area { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public string Logo { get; set; }
        public string MainImg { get; set; }
        public string MainColorHex { get; set; }
        public int Colaboradores { get; set; }
        public decimal Valor { get; set; }
        public decimal QntCotasTotais { get; set; }
        public decimal MarketCap { get; set; }
        public DateTime Fundacao { get; set; }
        public DateTime Abertura { get; set; }
        public string Sede { get; set; }
        public string Fundador { get; set; }
        public decimal? Dividendos { get; set; }
        public decimal VariacaoPercentual { get; set; }

        public InvestimentoOptionGenerico(InvestimentoOption option)
        {

            IdInvestimentoOption = option.IdInvestimentoOption;
            IdArea = option.IdAreaInvestimento;
            IdTipo = option.IdTipoInvestimento;
            Nome = option.Nome;
            Descricao = option.Descricao;
            Sigla = option.Sigla;
            Logo = option.Logo;
            MainImg = option.MainImg;
            MainColorHex = option.MainColorHex;
            Colaboradores = option.Colaboradores;
            Valor = option.ValorAcao;
            QntCotasTotais = option.QntCotasTotais;
            MarketCap = option.ValorAcao * option.QntCotasTotais;
            Fundacao = option.Fundacao.Date;
            Abertura = option.Abertura.Date;
            Sede = option.Sede;
            Fundador = option.Fundador;
            Dividendos = option.PercentualDividendos;
        }
    }
}

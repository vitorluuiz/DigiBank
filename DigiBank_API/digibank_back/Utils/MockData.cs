using Bogus;
using digibank_back.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace digibank_back.Utils
{
    public class MockData
    {
        public static InvestimentoOption MockAll(int sizeCompany)
        {
            var option = new InvestimentoOption();

            option = Identity.Get(option);
            option = Values.Get(option, sizeCompany);
            option = Date.Get(option, sizeCompany);
            option = Logo.Get(option);

            return option;
        }

        private static class Logo
        {
            public static InvestimentoOption Get(InvestimentoOption option)
            {
                List<int> list = NumberMagik.MergeIntRanges(new List<NumberRange>
                {
                    new NumberRange
                    {
                        Start = 223,
                        End = 226
                    }, new NumberRange
                    {
                        Start = 229,
                        End = 233,
                    }, new NumberRange
                    {
                        Start = 245,
                        End = 249
                    }, new NumberRange
                    {
                        Start = 266,
                        End = 283
                    }, new NumberRange
                    {
                        Start = 290,
                        End = 296
                    }, new NumberRange
                    {
                        Start = 298,
                        End = 299
                    }
                });

                Random random = new();
                int logoSkips = random.Next(1, list.Count);
                option.Logo = $"https://img.logoipsum.com/{list.Skip(logoSkips).First()}.svg";
                if (option.Nome.Length >= 6) option.Sigla = option.Nome[..6].ToUpper();

                return option;
            }
        }

        private class Date
        {
            public static InvestimentoOption Get(InvestimentoOption option, int sizeCompany)
            {
                int range = (int)Math.Round(Convert.ToDouble(sizeCompany) / 2);
                var faker = new Faker();
                option.Fundacao = faker.Date.Between(DateTime.Now.AddYears(-range), DateTime.Now.AddYears((int)Math.Round(-range * 1.5)));
                option.Abertura = faker.Date.Between(option.Fundacao, faker.Date.Between(option.Fundacao, option.Fundacao.AddYears(10)));
                if (option.Abertura > DateTime.Now.AddMonths(-6))
                {
                    option.Abertura = DateTime.Now.AddMonths(-6);
                }
                option.Tick = option.Abertura;

                return option;
            }
        }

        private class Values
        {
            public static InvestimentoOption Get(InvestimentoOption option, int sizeCompany)
            {
                int range = sizeCompany * 2;
                if (range > 90)
                {
                    range = range * 2;
                    if (range > 96)
                    {
                        range = range * 3;
                    }
                }
                var faker = new Faker();
                option.ValorAcao = faker.Random.Decimal(range, (decimal)(1.25 * range)); // P(1) 2-3 M(50) 100-150 G(85) 510-765 GG(95) 1710-2565 Max(100) 1800-2700
                option.PercentualDividendos = faker.Random.Decimal(0, (decimal)0.5) * 10; //0-5%
                option.Colaboradores = faker.Random.Int(15, 40) * range; //P(1)30-80 M(50) 1500-4000 G(85) 7650-20400 GG(95) 25650-68400 Max(100) 27000-72000
                option.QntCotasTotais = faker.Random.Int(6, 8) * range * 100000; //P(1)12-16 M(50) 600-800 G(85) 3060-5440 GG(95) 10260-13680 Max(100) 10800-14440

                return option;
            }
        }

        private class Identity
        {
            public static InvestimentoOption Get(InvestimentoOption option)
            {
                var faker = new Faker();
                option.Nome = $"{faker.Company.CompanyName()} {faker.Company.CompanySuffix()}";
                option.Descricao = $"{faker.Company.CatchPhrase()}";
                option.Fundador = $"{faker.Name.Prefix()} {faker.Name.FullName()}";
                option.Sede = $"{faker.Address.City()}, {faker.Address.Country()}";
                option.MainColorHex = faker.Random.Int(111111, 999999).ToString();
                option.MainImg = faker.Image.PicsumUrl(640, 480, false, false, null);
                option.IdAreaInvestimento = 3;
                option.IdTipoInvestimento = 3;

                return option;
            }
        }
    }
}

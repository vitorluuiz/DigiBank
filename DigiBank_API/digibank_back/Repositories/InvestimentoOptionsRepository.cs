using Bogus;
using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class InvestimentoOptionsRepository : IInvestimentoOptionsRepository
    {
        digiBankContext ctx = new digiBankContext();
        UsuarioRepository _usuarioRepository = new UsuarioRepository();
        public void Atualizar(int idInvestimentoOption, InvestimentoOption optionAtualizada)
        {
            InvestimentoOption optionDesatualizada = ListarPorId(idInvestimentoOption);

            optionDesatualizada.Nome = optionAtualizada.Nome;
            optionDesatualizada.IdTipoInvestimento = optionAtualizada.IdTipoInvestimento;
            optionDesatualizada.Sigla = optionAtualizada.Sigla;
            optionDesatualizada.ValorAcao = optionAtualizada.ValorAcao;
            optionDesatualizada.Descricao = optionAtualizada.Descricao;
            optionDesatualizada.PercentualDividendos = optionAtualizada.PercentualDividendos;

            ctx.Update(optionDesatualizada);
            ctx.SaveChanges();
        }

        public InvestimentoOption CreateFicOption()
        {
            InvestimentoOption antOption = ctx.InvestimentoOptions.FirstOrDefault(O => O.Nome == "Teste");

            if (antOption != null)
            {
                ctx.HistoricoInvestimentoOptions.RemoveRange(ctx.HistoricoInvestimentoOptions.Where(H => H.IdInvestimentoOption == antOption.IdInvestimentoOption));
                ctx.InvestimentoOptions.Remove(antOption);
                ctx.SaveChanges();
            }

            var lorem = new Bogus.DataSets.Lorem(locale: "pt_BR");
            Console.WriteLine(lorem.Sentence(5));

            InvestimentoOption option = new Faker<InvestimentoOption>("pt_BR")
                .RuleFor(o => o.Nome, p => $"{p.Company.CompanyName()} {p.Company.CompanySuffix()}")
                .RuleFor(o => o.Descricao, p => p.Company.CatchPhrase())
                .RuleFor(o => o.Abertura, p => p.Date.Past())
                .RuleFor(o => o.Fundacao, p => p.Date.Past())
                .RuleFor(o => o.Fundador, p => $"{p.Name.Prefix()} {p.Name.FullName()}")
                .RuleFor(o => o.ValorAcao, p => p.Random.Number(10, 400))
                .RuleFor(o => o.Colaboradores, p => p.Random.Int(30, 150000))
                .RuleFor(o => o.Sede, p => $"{p.Address.City()}, {p.Address.Country()}")
                .RuleFor(o => o.IdAreaInvestimento, p => p.Random.Short(1, ctx.AreaInvestimentos.OrderBy(a => a.IdAreaInvestimento).Last().IdAreaInvestimento))
                .RuleFor(o => o.IdTipoInvestimento, p => p.Random.Byte(1, ctx.TipoInvestimentos.OrderBy(a => a.IdTipoInvestimento).Last().IdTipoInvestimento))
                .RuleFor(o => o.Logo, "NaoTemLogoAinda.png")
                .RuleFor(o => o.MainImg, p => p.Image.PicsumUrl(640, 480, false, false, null))
                .RuleFor(o => o.MainColorHex, p => (p.Random.Int(111111, 999999)).ToString())
                .RuleFor(o => o.PercentualDividendos, p => p.Random.Decimal(0, 10))
                .RuleFor(o => o.QntCotasTotais, p => p.Random.Short(10000, short.MaxValue))
                .Generate();

            option.Sigla = option.Nome.Substring(0, 6).ToUpper();
            option.Tick = DateTime.Now.AddDays(-10);
            ctx.InvestimentoOptions.Add(option);
            ctx.SaveChanges();

            return option;
        }

        public void Deletar(int idInvestimentoOption)
        {
            ctx.Remove(ListarPorId(idInvestimentoOption));
        }

        public InvestimentoOption ListarPorId(int idInvestimentoOption)
        {
            return ctx.InvestimentoOptions
                .Include(f => f.IdTipoInvestimentoNavigation)
                .AsNoTracking()
                .FirstOrDefault(f => f.IdInvestimentoOption == idInvestimentoOption);
        }

        public List<InvestimentoOptionGenerico> ListarTodos(int pagina, int qntItens)
        {
            return ctx.InvestimentoOptions
                .Include(f => f.IdTipoInvestimentoNavigation)
                .Select( f => new InvestimentoOptionGenerico
                {
                    IdInvestimentoOption = f.IdInvestimentoOption,
                    IdTipoInvestimento = f.IdTipoInvestimentoNavigation.IdTipoInvestimento,
                    IdAreaInvestimento = f.IdAreaInvestimentoNavigation.IdAreaInvestimento,
                    Nome = f.Nome,
                    Colaboradores = f.Colaboradores,
                    ValorAcao = f.ValorAcao,
                    QntCotasTotais = f.QntCotasTotais,
                    Fundacao = f.Fundacao,
                    Abertura = f.Abertura,
                    Tick = f.Tick,
                }) 
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }
        //public List<PostGenerico> ListarCompradosAnteriormente(int pagina, int qntItens, int idUsuario)
        //{
        //    InventarioRepository inventarioRepository = new InventarioRepository();
        //    List<InvestimentoOptionGenerico> compradosAnteriormente = new List<InvestimentoOptionGenerico>();
        //    HashSet<int> idsAdicionados = new HashSet<int>();
        //    int paginacao = pagina;
        //    List<Inventario> inventario = new List<Inventario>();

        //    do
        //    {
        //        inventario = inventarioRepository.ListarMeuInventario(idUsuario, paginacao, qntItens);
        //        foreach (Inventario investimento in inventario)
        //        {
        //            if (investimento.IdPostNavigation.IsVirtual && investimento.IdPostNavigation.IsActive && !idsAdicionados.Contains(investimento.IdPost) && idsAdicionados.Count < qntItens)
        //            {
        //                string apelidoPropritario = ctx.Usuarios.FirstOrDefault(U => U.IdUsuario == investimento.Id.IdUsuario).Apelido;
        //                compradosAnteriormente.Add(new InvestimentoOptionGenerico
        //                {
        //                    IdInvestimentoOption = investimento.IdInvestimentoOption,
        //                    IdTipoInvestimento = investimento.IdTipoInvestimentoNavigation.IdTipoInvestimento,
        //                    IdAreaInvestimento = investimento.IdAreaInvestimentoNavigation.IdAreaInvestimento,
        //                    Nome = investimento.Nome,
        //                    ValorAcao = investimento.ValorAcao,
        //                    Tick = f.Tick,
        //                });

        //                idsAdicionados.Add(investimento.IdInvestimentoOption);
        //            }
        //        }

        //        paginacao++;
        //    } while (idsAdicionados.Count != qntItens && inventario.Count != 0);



        //    return compradosAnteriormente;
        //}
        public List<InvestimentoTitle> BuscarInvestimentos(int qntItens)
        {
            return ctx.InvestimentoOptions

                .Select(o => new InvestimentoTitle
                {
                    IdInvestimentoOption = o.IdInvestimentoOption,
                    Nome = o.Nome,
                    ValorAcao = o.ValorAcao,
                    Logo = o.Logo
                })
                .Take(qntItens)
                .ToList();
        }
        public List<InvestimentoOptionGenerico> ListarPorTipoInvestimento(byte idTipoInvestimentoOption, int pagina, int qntItens)
        {
            return ctx.InvestimentoOptions
                .Include(f => f.IdTipoInvestimentoNavigation)
                .Include(f => f.HistoricoInvestimentoOptions)
                .Include(f => f.Investimentos)
                .Where(f => f.IdTipoInvestimentoNavigation.IdTipoInvestimento == idTipoInvestimentoOption)
                .Select(f => new InvestimentoOptionGenerico
                {
                    IdInvestimentoOption = f.IdInvestimentoOption,
                    IdTipoInvestimento = f.IdTipoInvestimentoNavigation.IdTipoInvestimento,
                    IdAreaInvestimento = f.IdAreaInvestimentoNavigation.IdAreaInvestimento,
                    Nome = f.Nome,
                    Colaboradores = f.Colaboradores,
                    ValorAcao = f.ValorAcao,
                    QntCotasTotais = f.QntCotasTotais,
                    Fundacao = f.Fundacao,
                    Abertura = f.Abertura,
                    Tick = f.Tick,
                })
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }
        public bool Comprar(int idComprador, int idInvestimentoOption)
        {
            InvestimentoOption investimento = ListarPorId(idInvestimentoOption);

            if (investimento.IdUsuario == idComprador)
            {
                return false;
            }

            TransacaoRepository _transacaoRepository = new TransacaoRepository();
            Usuario comprador = _usuarioRepository.ListarPorId(idComprador);
            //Inventario inventario = new Inventario();

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Compra de {investimento.Nome}",
                Valor = investimento.ValorAcao,
                IdUsuarioPagante = Convert.ToInt16(idComprador),
                IdUsuarioRecebente = Convert.ToInt16(investimento.IdUsuario)
            };

            bool isSucess = _transacaoRepository.EfetuarTransacao(transacao);

            //if (isSucess)
            //{
            //    inventario.Valor = investimento.ValorAcao;
            //    inventario.IdPost = investimento.IdInvestimentoOption;
            //    inventario.IdUsuario = comprador.IdUsuario;

            //    _inventarioRepository.Depositar(inventario);

            //    investimento.Vendas++;

            //    ctx.Update(investimento);
            //    ctx.SaveChanges();

            //    return true;
            //}
            return false;
        }
    }
}
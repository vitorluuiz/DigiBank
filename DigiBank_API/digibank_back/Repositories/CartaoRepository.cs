using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Utils;
using digibank_back.ViewModel.Cartao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class CartaoRepository : ICartaoRepository
    {
        Random random = new Random();
        digiBankContext ctx = new digiBankContext();

        public bool AlterarSenha(int idCartao, string newToken)
        {
            Cartao cartao = ListarPorID(idCartao); 

            if(cartao == null )
            {
                return false;
            }

            if(Criptografia.CompararSenha(newToken, cartao.Token))
            {
                cartao.Token = newToken;

                ctx.Update(cartao);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Bloquear(int idCartao, string tokenModel)
        {
            Cartao cartao = ListarPorID(idCartao);

            if (cartao == null)
            {
                return false;
            }

            bool isAuth = Criptografia.CompararSenha(tokenModel, cartao.Token);

            if (isAuth)
            {
                cartao.IsValid = false;
                
                ctx.Update(cartao);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Desbloquear(int idCartao, string tokenModel)
        {
            Cartao cartao = ListarPorID(idCartao);

            if (cartao == null)
            {
                return false;
            }

            bool isAuth = Criptografia.CompararSenha(tokenModel, cartao.Token);

            if (isAuth)
            {
                cartao.IsValid = true;

                ctx.Update(cartao);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool EfetuarPagamento(CartaoViewModel cartaoModel)
        {

            Cartao cartao = ctx.Cartaos.FirstOrDefault(c => c.Numero == cartaoModel.Numero && c.Cvv == cartaoModel.Cvv);

            if(cartao == null)
            {
                return false;
            }

            bool isAuth = Criptografia.CompararSenha(cartaoModel.Token, cartao.Token);

            if (isAuth)
            {
                return true;
            }

            return false;
        }

        public Cartao Gerar(Cartao newCartao)
        {
            if (newCartao != null)
            {
                newCartao.DataExpira = DateTime.Now.AddDays(720);
                newCartao.Cvv = random.Next(100, 1000).ToString();
                for (int i = 0; i < 16; i++)
                {
                    newCartao.Numero += random.Next(0, 10).ToString();
                }
                newCartao.Token = Criptografia.CriptografarSenha(newCartao.Token);
                newCartao.IsValid = true;
                
                ctx.Add(newCartao);
                ctx.SaveChanges();

                return newCartao;
            }

            newCartao.IsValid = false;
            return newCartao;
        }

        public List<Cartao> GetCartoes(int idUsuario)
        {
           return ctx.Cartaos
                .Where(c => c.IdUsuario == idUsuario)
                .ToList();
        }

        public Cartao ListarPorID(int idCartao)
        {
            return ctx.Cartaos.FirstOrDefault(c => c.IdCartao == idCartao);
        }
    }
}

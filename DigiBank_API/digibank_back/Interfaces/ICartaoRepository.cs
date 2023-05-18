using digibank_back.Domains;
using digibank_back.ViewModel.Cartao;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface ICartaoRepository
    {
        Cartao Gerar(Cartao newCartao);
        bool Bloquear(int idCartao);
        bool Desbloquear(int idCartao);
        void Excluir(int idCartao);
        bool EfetuarPagamento(CartaoViewModel cartao);
        bool AlterarSenha(int idCartao, string newtoken);
        Cartao ListarPorID(int idCartao);
        List<Cartao> GetCartoes(int idUsuario);
    }
}

using digibank_back.Domains;
using digibank_back.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace digibank_back.Repositories
{
    public interface IMarketplaceRepository
    {
        Marketplace Cadastrar(Marketplace newPost);
        void Atualizar(Marketplace postAtualizado);
        bool Deletar(int idPost);
        List<PostMinimo> AllOrderBy(Expression<Func<Marketplace, decimal>> filter, int pagina, int qntItens, bool descending = false);
        List<PostMinimo> AllWhere(Expression<Func<Marketplace, bool>> predicate, int pagina, int qntItens);
        List<PostMinimo> TodosPorId(List<int> idsPosts);
        List<PostMinimo> CompradosAnteriormente(int pagina, int qntItens, int idUsuario);
        Marketplace PorId(int idPost, bool isOwner);
        PostGenerico PublicoPorId(int idPost, bool isOwner);
        List<PostMinimo> PublicoPorUsuario(int idUsuario);
        List<PostMinimo> Meus(int idUsuario);
        bool Comprar(int idComprador, int idPost);
        void TurnInative(int idPost);
        void TurnActive(int idPost);
        List<PostTitle> SearchBestResults(int qntItens);
    }
}

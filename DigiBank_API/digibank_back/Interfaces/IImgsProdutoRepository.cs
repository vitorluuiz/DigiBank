using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IImgsProdutoRepository
    {
        public void CadastrarCaminhos(byte idProduto, List<string> caminhos);
        public void DeletarCaminhos(byte idProduto);
        public List<string> ListarCaminhos(byte idProduto);
    }
}

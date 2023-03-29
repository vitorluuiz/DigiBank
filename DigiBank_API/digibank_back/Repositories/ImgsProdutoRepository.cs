using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class ImgsProdutoRepository : IImgsProdutoRepository
    {
        digiBankContext ctx = new digiBankContext();

        public void CadastrarCaminhos(byte idProduto, List<string> caminhos)
        {
            List<ImgsProduto> imgsProduto = new List<ImgsProduto>();

            for(int i = 0; i < caminhos.Count; i++)
            {
                ImgsProduto imgProduto = new ImgsProduto();
                imgProduto.IdProduto = idProduto;
                imgProduto.Img = caminhos[i];
                imgsProduto.Add(imgProduto);
            }
            ctx.ImgsProdutos.AddRange(imgsProduto);
            ctx.SaveChanges();
        }

        public void DeletarCaminhos(byte idProduto)
        {
            ctx.RemoveRange(ctx.ImgsProdutos.Where(i => i.IdProduto== idProduto));
            ctx.SaveChanges();
        }

        public List<string> ListarCaminhos(byte idProduto)
        {
            List<ImgsProduto> imgs = ctx.ImgsProdutos.Where(i => i.IdProduto == idProduto).ToList();
            List<string> caminhos = new List<string>();

            foreach(ImgsProduto img in imgs) 
            { 
                caminhos.Add(img.Img);
            }

            return caminhos;
        }
    }
}

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

        public void CadastrarCaminhos(byte idPost, List<string> caminhos)
        {
            List<ImgsPost> imgsPost = new List<ImgsPost>();

            for(int i = 0; i < caminhos.Count; i++)
            {
                ImgsPost imgPost = new ImgsPost();
                imgPost.IdPost = idPost;
                imgPost.Img = caminhos[i];
                imgsPost.Add(imgPost);
            }
            ctx.ImgsPosts.AddRange(imgsPost);
            ctx.SaveChanges();
        }

        public void DeletarCaminhos(byte idPost)
        {
            ctx.RemoveRange(ctx.ImgsPosts.Where(i => i.IdPost == idPost));
            ctx.SaveChanges();
        }

        public List<string> ListarCaminhos(byte idPost)
        {
            List<ImgsPost> imgs = ctx.ImgsPosts.Where(i => i.IdPost == idPost).ToList();
            List<string> caminhos = new List<string>();

            foreach(ImgsPost img in imgs) 
            { 
                caminhos.Add(img.Img);
            }

            return caminhos;
        }
    }
}

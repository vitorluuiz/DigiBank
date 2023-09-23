using digibank_back.Domains;
using System;

namespace digibank_back.DTOs
{
    public class AvaliacaoSimples
    {
        public int IdAvaliacao { get; set; }
        public int IdUsuario { get; set; }
        public byte IdPost { get; set; }
        public string Publicador { get; set; }
        public decimal Nota { get; set; }
        public short Replies { get; set; }
        public DateTime DataPostagem { get; set; }
        public string Comentario { get; set; }
        public bool IsReplied { get; set; }

        public AvaliacaoSimples(Avaliaco a, bool isReplied = false)
        {
            IdAvaliacao = a.IdAvaliacao;
            IdUsuario = a.IdUsuario;
            IdPost = a.IdPost;
            Publicador = a.IdUsuarioNavigation.Apelido;
            Nota = a.Nota;
            Replies = a.Replies;
            DataPostagem = a.DataPostagem;
            Comentario = a.Comentario;
            IsReplied = isReplied;
        }
    }
}

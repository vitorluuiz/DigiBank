using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Usuario
    {
        public Usuario()
        {
            Acos = new HashSet<Aco>();
            Avaliacos = new HashSet<Avaliaco>();
            Emprestimos = new HashSet<Emprestimo>();
            Fundos = new HashSet<Fundo>();
            Produtos = new HashSet<Produto>();
            TransacoIdUsuarioPaganteNavigations = new HashSet<Transaco>();
            TransacoIdUsuarioRecebenteNavigations = new HashSet<Transaco>();
        }

        public short IdUsuario { get; set; }
        public string NomeCompleto { get; set; }
        public string Apelido { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public decimal? DigiPoints { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? RendaFixa { get; set; }

        public virtual ICollection<Aco> Acos { get; set; }
        public virtual ICollection<Avaliaco> Avaliacos { get; set; }
        public virtual ICollection<Emprestimo> Emprestimos { get; set; }
        public virtual ICollection<Fundo> Fundos { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }
        public virtual ICollection<Transaco> TransacoIdUsuarioPaganteNavigations { get; set; }
        public virtual ICollection<Transaco> TransacoIdUsuarioRecebenteNavigations { get; set; }
    }
}

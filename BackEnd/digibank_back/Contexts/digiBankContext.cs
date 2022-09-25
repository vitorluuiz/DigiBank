using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using digibank_back.Domains;

#nullable disable

namespace digibank_back.Contexts
{
    public partial class digiBankContext : DbContext
    {
        public digiBankContext()
        {
        }

        public digiBankContext(DbContextOptions<digiBankContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aco> Acoes { get; set; }
        public virtual DbSet<AcoesOption> AcoesOptions { get; set; }
        public virtual DbSet<Avaliaco> Avaliacoes { get; set; }
        public virtual DbSet<Condico> Condicoes { get; set; }
        public virtual DbSet<Emprestimo> Emprestimos { get; set; }
        public virtual DbSet<EmprestimosOption> EmprestimosOptions { get; set; }
        public virtual DbSet<Fundo> Fundos { get; set; }
        public virtual DbSet<FundosOption> FundosOptions { get; set; }
        public virtual DbSet<Produto> Produtos { get; set; }
        public virtual DbSet<TiposFundo> TiposFundos { get; set; }
        public virtual DbSet<Transaco> Transacoes { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Data Source=DESKTOP-LEARTL9\\SQLExpress; initial catalog=DIGIBANK; user Id=sa; pwd=senai@132\n;");
                
                //Vitor
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-RR2ANFV\\SSDVITOR; initial catalog=DIGIBANK; Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Aco>(entity =>
            {
                entity.HasKey(e => e.IdAcao)
                    .HasName("PK__Acoes__7A3D732F360A5257");

                entity.Property(e => e.IdAcao).HasColumnName("idAcao");

                entity.Property(e => e.DataAquisicao).HasColumnType("datetime");

                entity.Property(e => e.IdAcoesOptions).HasColumnName("idAcoesOptions");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.ValorInicial).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdAcoesOptionsNavigation)
                    .WithMany(p => p.Acos)
                    .HasForeignKey(d => d.IdAcoesOptions)
                    .HasConstraintName("FK__Acoes__idAcoesOp__4222D4EF");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Acos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Acoes__idUsuario__412EB0B6");
            });

            modelBuilder.Entity<AcoesOption>(entity =>
            {
                entity.HasKey(e => e.IdAcaoOption)
                    .HasName("PK__AcoesOpt__5DB765FE2247C1EA");

                entity.HasIndex(e => e.Codigo, "UQ__AcoesOpt__06370DAC07C5054F")
                    .IsUnique();

                entity.HasIndex(e => e.Nome, "UQ__AcoesOpt__7D8FE3B2DC3DED68")
                    .IsUnique();

                entity.Property(e => e.IdAcaoOption)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idAcaoOption");

                entity.Property(e => e.AcaoImg)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Dividendos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Avaliaco>(entity =>
            {
                entity.HasKey(e => e.IdAvaliacao)
                    .HasName("PK__Avaliaco__2A0C83122F02489C");

                entity.Property(e => e.IdAvaliacao).HasColumnName("idAvaliacao");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdProduto).HasColumnName("idProduto");

                entity.Property(e => e.Nota).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdProdutoNavigation)
                    .WithMany(p => p.Avaliacos)
                    .HasForeignKey(d => d.IdProduto)
                    .HasConstraintName("FK__Avaliacoe__idPro__5BE2A6F2");
            });

            modelBuilder.Entity<Condico>(entity =>
            {
                entity.HasKey(e => e.IdCondicao)
                    .HasName("PK__Condicoe__EC5ECA4CF3D92340");

                entity.HasIndex(e => e.Condicao, "UQ__Condicoe__C18D4BAD207ADDC5")
                    .IsUnique();

                entity.Property(e => e.IdCondicao)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idCondicao");

                entity.Property(e => e.Condicao)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Emprestimo>(entity =>
            {
                entity.HasKey(e => e.IdEmprestimo)
                    .HasName("PK__Empresti__4B4C8860A2BFC9B6");

                entity.Property(e => e.IdEmprestimo).HasColumnName("idEmprestimo");

                entity.Property(e => e.DataFinal).HasColumnType("datetime");

                entity.Property(e => e.DataInicial).HasColumnType("datetime");

                entity.Property(e => e.IdCondicao).HasColumnName("idCondicao");

                entity.Property(e => e.IdEmprestimoOptions).HasColumnName("idEmprestimoOptions");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.HasOne(d => d.IdCondicaoNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdCondicao)
                    .HasConstraintName("FK__Emprestim__idCon__47DBAE45");

                entity.HasOne(d => d.IdEmprestimoOptionsNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdEmprestimoOptions)
                    .HasConstraintName("FK__Emprestim__idEmp__48CFD27E");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Emprestim__idUsu__46E78A0C");
            });

            modelBuilder.Entity<EmprestimosOption>(entity =>
            {
                entity.HasKey(e => e.IdEmprestimoOption)
                    .HasName("PK__Empresti__1400F9A113781E86");

                entity.Property(e => e.IdEmprestimoOption)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idEmprestimoOption");

                entity.Property(e => e.RendaMinima).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TaxaJuros).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<Fundo>(entity =>
            {
                entity.HasKey(e => e.IdFundo)
                    .HasName("PK__Fundos__A51E0073AD555C1A");

                entity.Property(e => e.IdFundo).HasColumnName("idFundo");

                entity.Property(e => e.DataFinal).HasColumnType("datetime");

                entity.Property(e => e.DataInicio).HasColumnType("datetime");

                entity.Property(e => e.DepositoInicial).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.IdfundosOptions).HasColumnName("idfundosOptions");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Fundos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Fundos__idUsuari__5165187F");

                entity.HasOne(d => d.IdfundosOptionsNavigation)
                    .WithMany(p => p.Fundos)
                    .HasForeignKey(d => d.IdfundosOptions)
                    .HasConstraintName("FK__Fundos__idfundos__52593CB8");
            });

            modelBuilder.Entity<FundosOption>(entity =>
            {
                entity.HasKey(e => e.IdFundosOption)
                    .HasName("PK__FundosOp__375F7150289AD308");

                entity.Property(e => e.IdFundosOption).HasColumnName("idFundosOption");

                entity.Property(e => e.Dividendos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IdTipoFundo).HasColumnName("idTipoFundo");

                entity.Property(e => e.IndiceConfiabilidade).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IndiceDividendos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IndiceValorizacao).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TaxaJuros).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdTipoFundoNavigation)
                    .WithMany(p => p.FundosOptions)
                    .HasForeignKey(d => d.IdTipoFundo)
                    .HasConstraintName("FK__FundosOpt__idTip__4E88ABD4");
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.IdProduto)
                    .HasName("PK__Produtos__5EEDF7C3256E9426");

                entity.Property(e => e.IdProduto)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idProduto");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ProdutoImg)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Produtos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Produtos__idUsua__59063A47");
            });

            modelBuilder.Entity<TiposFundo>(entity =>
            {
                entity.HasKey(e => e.IdTipoFundo)
                    .HasName("PK__TiposFun__536EAB28EE862282");

                entity.HasIndex(e => e.TipoFundo, "UQ__TiposFun__8F33A38F81BEEE97")
                    .IsUnique();

                entity.Property(e => e.IdTipoFundo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idTipoFundo");

                entity.Property(e => e.TipoFundo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transaco>(entity =>
            {
                entity.HasKey(e => e.IdTransacao)
                    .HasName("PK__Transaco__455E3CA03FE3AB4D");

                entity.Property(e => e.IdTransacao).HasColumnName("idTransacao");

                entity.Property(e => e.DataTransacao).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdUsuarioPagante).HasColumnName("idUsuarioPagante");

                entity.Property(e => e.IdUsuarioRecebente).HasColumnName("idUsuarioRecebente");

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdUsuarioPaganteNavigation)
                    .WithMany(p => p.TransacoIdUsuarioPaganteNavigations)
                    .HasForeignKey(d => d.IdUsuarioPagante)
                    .HasConstraintName("FK__Transacoe__idUsu__5535A963");

                entity.HasOne(d => d.IdUsuarioRecebenteNavigation)
                    .WithMany(p => p.TransacoIdUsuarioRecebenteNavigations)
                    .HasForeignKey(d => d.IdUsuarioRecebente)
                    .HasConstraintName("FK__Transacoe__idUsu__5629CD9C");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__645723A68651FCD3");

                entity.HasIndex(e => e.Telefone, "UQ__Usuarios__4EC504B6A5F9AEC6")
                    .IsUnique();

                entity.HasIndex(e => e.Cpf, "UQ__Usuarios__C1F897315FADB283")
                    .IsUnique();

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Apelido)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("CPF")
                    .IsFixedLength(true);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomeCompleto)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PontosVantagem).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.RendaFixa).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Senha)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

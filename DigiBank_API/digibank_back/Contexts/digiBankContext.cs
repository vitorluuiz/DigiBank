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
        public virtual DbSet<ImgsPost> ImgsPosts { get; set; }
        public virtual DbSet<Inventario> Inventarios { get; set; }
        public virtual DbSet<Marketplace> Marketplaces { get; set; }
        public virtual DbSet<TiposAco> TiposAcoes { get; set; }
        public virtual DbSet<TiposFundo> TiposFundos { get; set; }
        public virtual DbSet<Transaco> Transacoes { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=VITOR-PC; initial catalog=DIGIBANK; user Id=sa; pwd=Senai@132;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Aco>(entity =>
            {
                entity.HasKey(e => e.IdAcao)
                    .HasName("PK__Acoes__7A3D732F5E5A5B17");

                entity.Property(e => e.IdAcao).HasColumnName("idAcao");

                entity.Property(e => e.DataAquisicao).HasColumnType("datetime");

                entity.Property(e => e.IdAcoesOptions).HasColumnName("idAcoesOptions");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.ValorInicial).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdAcoesOptionsNavigation)
                    .WithMany(p => p.Acos)
                    .HasForeignKey(d => d.IdAcoesOptions)
                    .HasConstraintName("FK__Acoes__idAcoesOp__44FF419A");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Acos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Acoes__idUsuario__440B1D61");
            });

            modelBuilder.Entity<AcoesOption>(entity =>
            {
                entity.HasKey(e => e.IdAcaoOption)
                    .HasName("PK__AcoesOpt__5DB765FE539971C7");

                entity.HasIndex(e => e.Codigo, "UQ__AcoesOpt__06370DACF455BEA3")
                    .IsUnique();

                entity.HasIndex(e => e.Nome, "UQ__AcoesOpt__7D8FE3B2B379AF13")
                    .IsUnique();

                entity.Property(e => e.IdAcaoOption)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idAcaoOption");

                entity.Property(e => e.AcaoImg)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DividendoAnual).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IdTipoAcao).HasColumnName("idTipoAcao");

                entity.Property(e => e.IndiceConfiabilidade).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IndiceDividendos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IndiceValorizacao).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTipoAcaoNavigation)
                    .WithMany(p => p.AcoesOptions)
                    .HasForeignKey(d => d.IdTipoAcao)
                    .HasConstraintName("FK__AcoesOpti__idTip__412EB0B6");
            });

            modelBuilder.Entity<Avaliaco>(entity =>
            {
                entity.HasKey(e => e.IdAvaliacao)
                    .HasName("PK__Avaliaco__2A0C83125A5B438D");

                entity.Property(e => e.IdAvaliacao).HasColumnName("idAvaliacao");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nota).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Avaliacos)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK__Avaliacoe__idPos__5FB337D6");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Avaliacos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Avaliacoe__idUsu__5EBF139D");
            });

            modelBuilder.Entity<Condico>(entity =>
            {
                entity.HasKey(e => e.IdCondicao)
                    .HasName("PK__Condicoe__EC5ECA4C5E11D565");

                entity.HasIndex(e => e.Condicao, "UQ__Condicoe__C18D4BAD36E837C1")
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
                    .HasName("PK__Empresti__4B4C8860966809C3");

                entity.Property(e => e.IdEmprestimo).HasColumnName("idEmprestimo");

                entity.Property(e => e.DataFinal).HasColumnType("datetime");

                entity.Property(e => e.DataInicial).HasColumnType("datetime");

                entity.Property(e => e.IdCondicao).HasColumnName("idCondicao");

                entity.Property(e => e.IdEmprestimoOptions).HasColumnName("idEmprestimoOptions");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.UltimoPagamento).HasColumnType("datetime");

                entity.Property(e => e.ValorPago).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdCondicaoNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdCondicao)
                    .HasConstraintName("FK__Emprestim__idCon__4AB81AF0");

                entity.HasOne(d => d.IdEmprestimoOptionsNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdEmprestimoOptions)
                    .HasConstraintName("FK__Emprestim__idEmp__4BAC3F29");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Emprestim__idUsu__49C3F6B7");
            });

            modelBuilder.Entity<EmprestimosOption>(entity =>
            {
                entity.HasKey(e => e.IdEmprestimoOption)
                    .HasName("PK__Empresti__1400F9A1A9BA6C5F");

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
                    .HasName("PK__Fundos__A51E00730FFA37A3");

                entity.Property(e => e.IdFundo).HasColumnName("idFundo");

                entity.Property(e => e.DataInicio).HasColumnType("datetime");

                entity.Property(e => e.DepositoInicial).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.IdfundosOptions).HasColumnName("idfundosOptions");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Fundos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Fundos__idUsuari__5441852A");

                entity.HasOne(d => d.IdfundosOptionsNavigation)
                    .WithMany(p => p.Fundos)
                    .HasForeignKey(d => d.IdfundosOptions)
                    .HasConstraintName("FK__Fundos__idfundos__5535A963");
            });

            modelBuilder.Entity<FundosOption>(entity =>
            {
                entity.HasKey(e => e.IdFundosOption)
                    .HasName("PK__FundosOp__375F7150233F552C");

                entity.Property(e => e.IdFundosOption).HasColumnName("idFundosOption");

                entity.Property(e => e.IdTipoFundo).HasColumnName("idTipoFundo");

                entity.Property(e => e.IndiceConfiabilidade).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IndiceDividendos).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IndiceValorizacao).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.NomeFundo)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TaxaJuros).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdTipoFundoNavigation)
                    .WithMany(p => p.FundosOptions)
                    .HasForeignKey(d => d.IdTipoFundo)
                    .HasConstraintName("FK__FundosOpt__idTip__5165187F");
            });

            modelBuilder.Entity<ImgsPost>(entity =>
            {
                entity.HasKey(e => e.IdImg)
                    .HasName("PK__ImgsPost__3C3EAB5ABA824F9D");

                entity.ToTable("ImgsPost");

                entity.Property(e => e.IdImg).HasColumnName("idImg");

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.Img)
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.ImgsPosts)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK__ImgsPost__idPost__628FA481");
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(e => e.IdInventario)
                    .HasName("PK__Inventar__8F145B0D680760A7");

                entity.ToTable("Inventario");

                entity.Property(e => e.IdInventario).HasColumnName("idInventario");

                entity.Property(e => e.DataAquisicao).HasColumnType("datetime");

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdPost)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventari__idPos__66603565");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventari__idUsu__656C112C");
            });

            modelBuilder.Entity<Marketplace>(entity =>
            {
                entity.HasKey(e => e.IdPost)
                    .HasName("PK__Marketpl__BE0F4FD6309F7EC3");

                entity.ToTable("Marketplace");

                entity.Property(e => e.IdPost)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idPost");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.IsVirtual).HasColumnName("isVirtual");

                entity.Property(e => e.IsVisible).HasColumnName("isVisible");

                entity.Property(e => e.MainImg)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Marketplaces)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Marketpla__idUsu__5BE2A6F2");
            });

            modelBuilder.Entity<TiposAco>(entity =>
            {
                entity.HasKey(e => e.IdTipoAcao)
                    .HasName("PK__TiposAco__130C48C156D2E9ED");

                entity.Property(e => e.IdTipoAcao)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idTipoAcao");

                entity.Property(e => e.TipoAcao)
                    .HasMaxLength(35)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TiposFundo>(entity =>
            {
                entity.HasKey(e => e.IdTipoFundo)
                    .HasName("PK__TiposFun__536EAB2801BCD9CB");

                entity.HasIndex(e => e.TipoFundo, "UQ__TiposFun__8F33A38FBF392CD4")
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
                    .HasName("PK__Transaco__455E3CA0317FFE92");

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
                    .HasConstraintName("FK__Transacoe__idUsu__5812160E");

                entity.HasOne(d => d.IdUsuarioRecebenteNavigation)
                    .WithMany(p => p.TransacoIdUsuarioRecebenteNavigations)
                    .HasForeignKey(d => d.IdUsuarioRecebente)
                    .HasConstraintName("FK__Transacoe__idUsu__59063A47");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__645723A67C112439");

                entity.HasIndex(e => e.Telefone, "UQ__Usuarios__4EC504B62B720EC7")
                    .IsUnique();

                entity.HasIndex(e => e.Cpf, "UQ__Usuarios__C1F89731AE9C632D")
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

                entity.Property(e => e.DigiPoints).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomeCompleto)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RendaFixa).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Saldo).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .IsFixedLength(true);

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

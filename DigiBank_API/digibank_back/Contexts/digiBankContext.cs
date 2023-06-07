using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using digibank_back.Domains;
using System.Collections.Generic;

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

        public virtual DbSet<Avaliaco> Avaliacoes { get; set; }
        public virtual DbSet<Cartao> Cartaos { get; set; }
        public virtual DbSet<Condico> Condicoes { get; set; }
        public virtual DbSet<Emprestimo> Emprestimos { get; set; }
        public virtual DbSet<EmprestimosOption> EmprestimosOptions { get; set; }
        public virtual DbSet<ImgsPost> ImgsPosts { get; set; }
        public virtual DbSet<Inventario> Inventarios { get; set; }
        public virtual DbSet<Investimento> Investimentos { get; set; }
        public virtual DbSet<InvestimentoOption> InvestimentoOptions { get; set; }
        public virtual DbSet<Marketplace> Marketplaces { get; set; }
        public virtual DbSet<Meta> Metas { get; set; }
        public virtual DbSet<TipoInvestimento> TipoInvestimentos { get; set; }
        public virtual DbSet<Transaco> Transacoes { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public IEnumerable<object> ImagensPosts { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=VITOR-PC; initial catalog=DIGIBANK; user Id=sa; pwd=senai@132;");
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-LEARTL9\\SQLExpress; initial catalog=DIGIBANK; user Id=sa; pwd=senai@132;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Avaliaco>(entity =>
            {
                entity.HasKey(e => e.IdAvaliacao)
                    .HasName("PK__Avaliaco__2A0C8312F597A74A");

                entity.Property(e => e.IdAvaliacao).HasColumnName("idAvaliacao");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Nota).HasColumnType("decimal(2, 1)");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Avaliacos)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK__Avaliacoe__idPos__5EBF139D");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Avaliacos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Avaliacoe__idUsu__5DCAEF64");
            });

            modelBuilder.Entity<Cartao>(entity =>
            {
                entity.HasKey(e => e.IdCartao)
                    .HasName("PK__Cartao__C212DE25728BE376");

                entity.ToTable("Cartao");

                entity.HasIndex(e => e.Numero, "UQ__Cartao__7E532BC6958894A5")
                    .IsUnique();

                entity.Property(e => e.IdCartao).HasColumnName("idCartao");

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("CVV")
                    .IsFixedLength(true);

                entity.Property(e => e.DataExpira).HasColumnType("datetime");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.IsValid).HasColumnName("isValid");

                entity.Property(e => e.Nome)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Cartaos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Cartao__idUsuari__3F466844");
            });

            modelBuilder.Entity<Condico>(entity =>
            {
                entity.HasKey(e => e.IdCondicao)
                    .HasName("PK__Condicoe__EC5ECA4CA66514E1");

                entity.HasIndex(e => e.Condicao, "UQ__Condicoe__C18D4BADC75E1825")
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
                    .HasName("PK__Empresti__4B4C88602E640B7A");

                entity.Property(e => e.IdEmprestimo).HasColumnName("idEmprestimo");

                entity.Property(e => e.DataFinal).HasColumnType("datetime");

                entity.Property(e => e.DataInicial).HasColumnType("datetime");

                entity.Property(e => e.IdCondicao).HasColumnName("idCondicao");

                entity.Property(e => e.IdEmprestimoOptions).HasColumnName("idEmprestimoOptions");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.UltimoPagamento).HasColumnType("datetime");

                entity.Property(e => e.ValorPago).HasColumnType("decimal(9, 2)");

                entity.HasOne(d => d.IdCondicaoNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdCondicao)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Emprestim__idCon__47DBAE45");

                entity.HasOne(d => d.IdEmprestimoOptionsNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdEmprestimoOptions)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Emprestim__idEmp__48CFD27E");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Emprestimos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Emprestim__idUsu__46E78A0C");
            });

            modelBuilder.Entity<EmprestimosOption>(entity =>
            {
                entity.HasKey(e => e.IdEmprestimoOption)
                    .HasName("PK__Empresti__1400F9A142FE3294");

                entity.Property(e => e.IdEmprestimoOption)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idEmprestimoOption");

                entity.Property(e => e.RendaMinima).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.TaxaJuros).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.Valor).HasColumnType("decimal(9, 2)");
            });

            modelBuilder.Entity<ImgsPost>(entity =>
            {
                entity.HasKey(e => e.IdImg)
                    .HasName("PK__ImgsPost__3C3EAB5AC101DB14");

                entity.ToTable("ImgsPost");

                entity.Property(e => e.IdImg).HasColumnName("idImg");

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.Img)
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.ImgsPosts)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK__ImgsPost__idPost__619B8048");
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(e => e.IdInventario)
                    .HasName("PK__Inventar__8F145B0D925DDEF4");

                entity.ToTable("Inventario");

                entity.Property(e => e.IdInventario).HasColumnName("idInventario");

                entity.Property(e => e.DataAquisicao).HasColumnType("datetime");

                entity.Property(e => e.IdPost).HasColumnName("idPost");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Valor).HasColumnType("decimal(9, 2)");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdPost)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventari__idPos__656C112C");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Inventarios)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventari__idUsu__6477ECF3");
            });

            modelBuilder.Entity<Investimento>(entity =>
            {
                entity.HasKey(e => e.IdInvestimento)
                    .HasName("PK__Investim__93C8510B97CF5584");

                entity.ToTable("Investimento");

                entity.Property(e => e.IdInvestimento).HasColumnName("idInvestimento");

                entity.Property(e => e.DataAquisicao).HasColumnType("datetime");

                entity.Property(e => e.DepositoInicial).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.IdInvestimentoOption).HasColumnName("idInvestimentoOption");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.QntCotas).HasColumnType("decimal(12, 7)");

                entity.HasOne(d => d.IdInvestimentoOptionNavigation)
                    .WithMany(p => p.Investimentos)
                    .HasForeignKey(d => d.IdInvestimentoOption)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Investime__idInv__5441852A");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Investimentos)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Investime__idUsu__534D60F1");
            });

            modelBuilder.Entity<InvestimentoOption>(entity =>
            {
                entity.HasKey(e => e.IdInvestimentoOption)
                    .HasName("PK__Investim__DA79A85EBB5018BA");

                entity.HasIndex(e => e.Nome, "UQ__Investim__7D8FE3B2FF9B7C34")
                    .IsUnique();

                entity.HasIndex(e => e.CodeId, "UQ__Investim__C6DE2C1469A1FD42")
                    .IsUnique();

                entity.Property(e => e.IdInvestimentoOption).HasColumnName("idInvestimentoOption");

                entity.Property(e => e.CodeId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(240)
                    .IsUnicode(false);

                entity.Property(e => e.Dividendos).HasColumnType("decimal(4, 2)");

                entity.Property(e => e.IdTipoInvestimento).HasColumnName("idTipoInvestimento");

                entity.Property(e => e.Img)
                    .IsRequired()
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.IndiceConfiabilidade).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.IndiceDividendos).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.IndiceValorizacao).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ValorInicial).HasColumnType("decimal(9, 2)");

                entity.HasOne(d => d.IdTipoInvestimentoNavigation)
                    .WithMany(p => p.InvestimentoOptions)
                    .HasForeignKey(d => d.IdTipoInvestimento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Investime__idTip__5070F446");
            });

            modelBuilder.Entity<Marketplace>(entity =>
            {
                entity.HasKey(e => e.IdPost)
                    .HasName("PK__Marketpl__BE0F4FD624E23BEE");

                entity.ToTable("Marketplace");

                entity.Property(e => e.IdPost)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idPost");

                entity.Property(e => e.Avaliacao).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsVirtual).HasColumnName("isVirtual");

                entity.Property(e => e.MainImg)
                    .IsRequired()
                    .HasMaxLength(41)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("decimal(9, 2)");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Marketplaces)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Marketpla__idUsu__5AEE82B9");
            });

            modelBuilder.Entity<Meta>(entity =>
            {
                entity.HasKey(e => e.IdMeta)
                    .HasName("PK__Metas__C26D05DE07E5FC63");

                entity.Property(e => e.IdMeta).HasColumnName("idMeta");

                entity.Property(e => e.Arrecadado).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.ValorMeta).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Meta)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK__Metas__idUsuario__4222D4EF");
            });

            modelBuilder.Entity<TipoInvestimento>(entity =>
            {
                entity.HasKey(e => e.IdTipoInvestimento)
                    .HasName("PK__TipoInve__7024AB4C8073AC4D");

                entity.HasIndex(e => e.TipoInvestimento1, "UQ__TipoInve__C197F226C33FDED8")
                    .IsUnique();

                entity.Property(e => e.IdTipoInvestimento)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idTipoInvestimento");

                entity.Property(e => e.TipoInvestimento1)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("TipoInvestimento");
            });

            modelBuilder.Entity<Transaco>(entity =>
            {
                entity.HasKey(e => e.IdTransacao)
                    .HasName("PK__Transaco__455E3CA0F67EFEA1");

                entity.Property(e => e.IdTransacao).HasColumnName("idTransacao");

                entity.Property(e => e.DataTransacao).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IdUsuarioPagante).HasColumnName("idUsuarioPagante");

                entity.Property(e => e.IdUsuarioRecebente).HasColumnName("idUsuarioRecebente");

                entity.Property(e => e.Valor).HasColumnType("decimal(10, 3)");

                entity.HasOne(d => d.IdUsuarioPaganteNavigation)
                    .WithMany(p => p.TransacoIdUsuarioPaganteNavigations)
                    .HasForeignKey(d => d.IdUsuarioPagante)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacoe__idUsu__571DF1D5");

                entity.HasOne(d => d.IdUsuarioRecebenteNavigation)
                    .WithMany(p => p.TransacoIdUsuarioRecebenteNavigations)
                    .HasForeignKey(d => d.IdUsuarioRecebente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Transacoe__idUsu__5812160E");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuarios__645723A62266A5AE");

                entity.HasIndex(e => e.Telefone, "UQ__Usuarios__4EC504B6D4BCD800")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D105349A115A5D")
                    .IsUnique();

                entity.HasIndex(e => e.Cpf, "UQ__Usuarios__C1F89731115BDB57")
                    .IsUnique();

                entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");

                entity.Property(e => e.Apelido)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Cpf)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("CPF")
                    .IsFixedLength(true);

                entity.Property(e => e.DigiPoints).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.NomeCompleto)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RendaFixa).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Saldo).HasColumnType("decimal(11, 3)");

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

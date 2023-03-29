using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apiBlog.Models;

public partial class SeBlogContext : DbContext
{
    public SeBlogContext()
    {
    }

    public SeBlogContext(DbContextOptions<SeBlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAutor> TbAutors { get; set; } = null!;

    public virtual DbSet<TbNoticia> TbNoticias { get; set; } = null!;

    public virtual DbSet<TbStatusNoticia> TbStatusNoticias { get; set; } = null!;

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAutor>(entity =>
        {
            entity.HasKey(e => e.Idautor).HasName("PK__TB_Autor__5DC53A138E212ECF");

            entity.ToTable("TB_Autor");

            entity.Property(e => e.Idautor).HasColumnName("IDAutor");
            entity.Property(e => e.ApelidoAutor)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Codusuario).HasColumnName("CODUsuario");
            entity.Property(e => e.SenhaProvisoria).IsUnicode(false);

            entity.HasOne(d => d.CodusuarioNavigation).WithMany(p => p.TbAutors)
                .HasForeignKey(d => d.Codusuario)
                .HasConstraintName("FK__TB_Autore__CODUs__4F7CD00D");
        });

        modelBuilder.Entity<TbNoticia>(entity =>
        {
            entity.HasKey(e => e.Idnoticia).HasName("PK__TB_Notic__A7F535DB1100CE6E");

            entity.ToTable("TB_Noticias");

            entity.Property(e => e.Idnoticia).HasColumnName("IDNoticia");
            entity.Property(e => e.Codautor).HasColumnName("CODAutor");
            entity.Property(e => e.DataAlteracao).HasColumnType("date");
            entity.Property(e => e.DataPublicacao).HasColumnType("date");
            entity.Property(e => e.Situacao)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.SubTitulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Texto).IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.CodautorNavigation).WithMany(p => p.TbNoticia)
                .HasForeignKey(d => d.Codautor)
                .HasConstraintName("FK__TB_Notici__CODAu__52593CB8");
        });

        modelBuilder.Entity<TbStatusNoticia>(entity =>
        {
            entity.HasKey(e => e.IdstatusNoticia).HasName("PK__TB_Statu__A88FCFC093DF7835");

            entity.ToTable("TB_StatusNoticias");

            entity.Property(e => e.IdstatusNoticia).HasColumnName("IDStatusNoticia");
            entity.Property(e => e.Codleitor).HasColumnName("CODLeitor");
            entity.Property(e => e.Codnoticia).HasColumnName("CODNoticia");
            entity.Property(e => e.Comentario).IsUnicode(false);

            entity.HasOne(d => d.CodleitorNavigation).WithMany(p => p.TbStatusNoticia)
                .HasForeignKey(d => d.Codleitor)
                .HasConstraintName("FK__TB_Status__CODLe__5629CD9C");

            entity.HasOne(d => d.CodnoticiaNavigation).WithMany(p => p.TbStatusNoticia)
                .HasForeignKey(d => d.Codnoticia)
                .HasConstraintName("FK__TB_Status__CODNo__5535A963");
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__TB_Usuar__523111691F73F486");

            entity.ToTable("TB_Usuarios");

            entity.HasIndex(e => e.Email, "UQ__TB_Usuar__A9D10534EEB17149").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.CodAtivacao)
                .IsUnicode(false)
                .HasColumnName("codAtivacao");
            entity.Property(e => e.DtNascimento)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("dtNascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nome");
            entity.Property(e => e.Senha)
                .IsUnicode(false)
                .HasColumnName("senha");
            entity.Property(e => e.StatusConta)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("statusConta");
            entity.Property(e => e.StatusSenha)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("statusSenha");
            entity.Property(e => e.TipoUsuario)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("tipoUsuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

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

    public virtual DbSet<TbAutore> TbAutores { get; set; }

    public virtual DbSet<TbNoticia> TbNoticias { get; set; }

    public virtual DbSet<TbStatusNoticia> TbStatusNoticias { get; set; }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Development");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAutore>(entity =>
        {
            entity.HasKey(e => e.Idautor).HasName("PK__TB_Autor__5DC53A1333FA8104");

            entity.ToTable("TB_Autores");

            entity.Property(e => e.Idautor).HasColumnName("IDAutor");
            entity.Property(e => e.ApelidoAutor)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Codusuario).HasColumnName("CODUsuario");
            entity.Property(e => e.SenhaProvisoria).IsUnicode(false);

            entity.HasOne(d => d.CodusuarioNavigation).WithMany(p => p.TbAutores)
                .HasForeignKey(d => d.Codusuario)
                .HasConstraintName("FK__TB_Autore__CODUs__3A81B327");
        });

        modelBuilder.Entity<TbNoticia>(entity =>
        {
            entity.HasKey(e => e.Idnoticia).HasName("PK__TB_Notic__A7F535DB427645C4");

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
                .HasConstraintName("FK__TB_Notici__CODAu__3D5E1FD2");
        });

        modelBuilder.Entity<TbStatusNoticia>(entity =>
        {
            entity.HasKey(e => e.IdstatusNoticia).HasName("PK__TB_Statu__A88FCFC00DC6FD1D");

            entity.ToTable("TB_StatusNoticias");

            entity.Property(e => e.IdstatusNoticia).HasColumnName("IDStatusNoticia");
            entity.Property(e => e.Codleitor).HasColumnName("CODLeitor");
            entity.Property(e => e.Codnoticia).HasColumnName("CODNoticia");
            entity.Property(e => e.Comentario).IsUnicode(false);
            entity.Property(e => e.DtComentario).HasColumnType("date");

            entity.HasOne(d => d.CodleitorNavigation).WithMany(p => p.TbStatusNoticia)
                .HasForeignKey(d => d.Codleitor)
                .HasConstraintName("FK__TB_Status__CODLe__412EB0B6");

            entity.HasOne(d => d.CodnoticiaNavigation).WithMany(p => p.TbStatusNoticia)
                .HasForeignKey(d => d.Codnoticia)
                .HasConstraintName("FK__TB_Status__CODNo__403A8C7D");
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("PK__TB_Usuar__52311169036018A0");

            entity.ToTable("TB_Usuarios");

            entity.HasIndex(e => e.Email, "UQ__TB_Usuar__A9D105343350C53B").IsUnique();

            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");
            entity.Property(e => e.CodAtivacao)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.CodSenha)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.DtaltSenha)
                .HasColumnType("datetime")
                .HasColumnName("DTAltSenha");
            entity.Property(e => e.Dtnascimento)
                .HasColumnType("date")
                .HasColumnName("DTNascimento");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Senha).HasColumnType("text");
            entity.Property(e => e.StatusConta)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.StatusSenha)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TipoUsuario)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

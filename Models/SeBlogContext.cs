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

    public virtual DbSet<TbNoticia> TbNoticias { get; set; }

    public virtual DbSet<TbStatus> TbStatuses { get; set; }

    public virtual DbSet<TbStatusNoticia> TbStatusNoticias { get; set; }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Development");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbNoticia>(entity =>
        {
            entity.HasKey(e => e.Idnoticia).HasName("PK__TB_Notic__A7F535DB5797BCEA");

            entity.ToTable("TB_Noticias");

            entity.Property(e => e.Idnoticia).HasColumnName("IDNoticia");
            entity.Property(e => e.Codautor).HasColumnName("CODAutor");
            entity.Property(e => e.DataAlteracao).HasColumnType("datetime");
            entity.Property(e => e.DataPublicacao).HasColumnType("datetime");
            entity.Property(e => e.Situacao)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Subtitulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Texto).IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.CodautorNavigation).WithMany(p => p.TbNoticia)
                .HasForeignKey(d => d.Codautor)
                .HasConstraintName("FK__TB_Notici__CODAu__3A81B327");
        });

        modelBuilder.Entity<TbStatus>(entity =>
        {
            entity.HasKey(e => e.Idstatus).HasName("PK__TB_Statu__8DA24510702DA321");

            entity.ToTable("TB_Status");

            entity.Property(e => e.Idstatus).HasColumnName("IDStatus");
            entity.Property(e => e.NomeStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbStatusNoticia>(entity =>
        {
            entity.HasKey(e => e.IdstatusNoticia).HasName("PK__TB_Statu__A88FCFC05F4740FC");

            entity.ToTable("TB_StatusNoticias");

            entity.Property(e => e.IdstatusNoticia).HasColumnName("IDStatusNoticia");
            entity.Property(e => e.Codleitor).HasColumnName("CODLeitor");
            entity.Property(e => e.Codnoticia).HasColumnName("CODNoticia");
            entity.Property(e => e.Codstatus).HasColumnName("CODStatus");
            entity.Property(e => e.Comentario).IsUnicode(false);
            entity.Property(e => e.DtComentario).HasColumnType("datetime");

            entity.HasOne(d => d.CodleitorNavigation).WithMany(p => p.TbStatusNoticia)
                .HasForeignKey(d => d.Codleitor)
                .HasConstraintName("FK__TB_Status__CODLe__571DF1D5");

            entity.HasOne(d => d.CodnoticiaNavigation).WithMany(p => p.TbStatusNoticia)
                .HasForeignKey(d => d.Codnoticia)
                .HasConstraintName("FK__TB_Status__CODNo__5629CD9C");

            entity.HasOne(d => d.CodstatusNavigation).WithMany(p => p.TbStatusNoticia)
                .HasForeignKey(d => d.Codstatus)
                .HasConstraintName("FK__TB_Status__CODSt__5812160E");
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("PK__TB_Usuar__52311169265F00BC");

            entity.ToTable("TB_Usuarios");

            entity.HasIndex(e => e.Email, "UQ__TB_Usuar__A9D10534DD06A555").IsUnique();

            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");
            entity.Property(e => e.ApelidoAutor)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.CodAtivacao).IsUnicode(false);
            entity.Property(e => e.CodSenha).IsUnicode(false);
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
            entity.Property(e => e.Senha).IsUnicode(false);
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

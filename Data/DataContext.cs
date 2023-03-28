using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Usuarios> TBUsuario { get; set; } = null!;
    public DbSet<Autores> TB_Autores { get; set; } = null!;
}
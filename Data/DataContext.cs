using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Usuarios> TB_Usuarios { get; set; } = null!;
    public DbSet<Autores> TB_Autores { get; set; } = null!;
}
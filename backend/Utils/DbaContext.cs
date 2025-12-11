using Microsoft.EntityFrameworkCore;
using Models;

namespace Utils;

public class DbaContext : DbContext
{
    public DbaContext(DbContextOptions<DbaContext> o): base(o) {}

    public DbSet<Genre> Genres { get; set; }
    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Tentative> Tentatives { get; set; }
}

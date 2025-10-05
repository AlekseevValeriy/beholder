using BeholderServer.Models;

using Microsoft.EntityFrameworkCore;

namespace BeholderServer;

public class TeleprogramDB : DbContext
{
    const String SERVER = @"LAPTOP-O1SR6LRJ\MSSQLSERVER01";
    const String DATABASE = @"Teleprogram";

    public TeleprogramDB(DbContextOptions<TeleprogramDB> options) : base(options) { }

    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<TVProgram> Programs => Set<TVProgram>();
    public DbSet<Schedule> Schedule => Set<Schedule>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@$"Server={SERVER}; Database={DATABASE}; TrustServerCertificate=True; Trusted_Connection=True;");
    }
}

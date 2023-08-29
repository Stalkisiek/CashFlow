global using Microsoft.EntityFrameworkCore;
using CashFlow.Models;

namespace CashFlow.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Mapowanie dla klasy User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users"); // Nazwa tabeli w bazie danych

            entity.HasKey(e => e.Id); // Klucz główny

            // Mapowanie pól
            entity.Property(e => e.Id).HasColumnName("Id");
            entity.Property(e => e.Name).HasColumnName("Name");
            entity.Property(e => e.Surname).HasColumnName("Surname");
            entity.Property(e => e.Email).HasColumnName("Email");
            entity.Property(e => e.PasswordHash).HasColumnName("PasswordHash");
            entity.Property(e => e.PasswordSalt).HasColumnName("PasswordSalt");
            entity.Property(e => e.AuthorizationLevel).HasColumnName("AuthorizationLevel");
            entity.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
            entity.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        });
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<PreviousRequest> PreviousRequests => Set<PreviousRequest>();
}
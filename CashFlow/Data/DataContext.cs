global using Microsoft.EntityFrameworkCore;
using CashFlow.Models;

namespace CashFlow.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users => Set<User>();
}
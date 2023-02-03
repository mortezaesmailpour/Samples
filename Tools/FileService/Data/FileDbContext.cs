using Microsoft.EntityFrameworkCore;
using Tools.Model;

namespace Tools;

public class FileDbContext : DbContext
{
    public DbSet<FileModel> Files { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=.\;Database=EFCoreDemo;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
}
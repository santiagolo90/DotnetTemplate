using DotnetTemplate.EntityFramework.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotnetTemplate.EntityFramework.Shared.DbContexts
{
    public class DotnetTemplateDbContext : DbContext
    {
        public DotnetTemplateDbContext(DbContextOptions<DotnetTemplateDbContext> options)
        : base(options)
        {
            
        }
        public DbSet<AuditDto> Audit { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuraciones adicionales
        }
    }
}

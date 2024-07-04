
using DotnetTemplate.EntityFramework.Shared.DbContexts;
using DotnetTemplate.EntityFramework.Shared.Entities;
using DotnetTemplate.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetTemplate.Infraestructure.Repository
{
    public class AuditRepository : IAuditRepository
    {
        private readonly DotnetTemplateDbContext _dotnetTemplateDbContext;
        public AuditRepository(DotnetTemplateDbContext dotnetTemplateDbContext)
        {
            _dotnetTemplateDbContext = dotnetTemplateDbContext;
        }
        public async Task<List<AuditDto>> GetAllAsync()
        {
            var audits = await _dotnetTemplateDbContext.Audit.ToListAsync();
            if (audits == null || audits.Count <= 0)
            {
                audits = new List<AuditDto>();
            }
            return audits;
        }

        public async Task AddAsync(AuditDto auditDto)
        {
            await _dotnetTemplateDbContext.Audit.AddAsync(auditDto);
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            await _dotnetTemplateDbContext.SaveChangesAsync();
        }
    }
}

using DotnetTemplate.EntityFramework.Shared.Entities;

namespace DotnetTemplate.Infraestructure.Interfaces
{
    public interface IAuditRepository
    {
        Task<List<AuditDto>> GetAllAsync();
        Task AddAsync(AuditDto auditDto);
    }
}



using DotnetTemplate.EntityFramework.Shared.Entities;

namespace DotnetTemplate.Application.Interfaces
{
    public interface IAuditService
    {
        Task<List<AuditDto>> GetAllAsync();
        Task AddAsync(AuditDto auditDto);
    }
}

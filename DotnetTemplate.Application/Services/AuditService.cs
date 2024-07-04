
using DotnetTemplate.Application.Interfaces;
using DotnetTemplate.EntityFramework.Shared.Entities;
using DotnetTemplate.Infraestructure.Interfaces;

namespace DotnetTemplate.Application.Services
{
    public class AuditService: IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        public AuditService(IAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }
        public Task<List<AuditDto>> GetAllAsync()
        {
            return _auditRepository.GetAllAsync();
        }

        public Task AddAsync(AuditDto auditDto)
        {
            return _auditRepository.AddAsync(auditDto);
        }
    }
}

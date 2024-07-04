using DotnetTemplate.Application.Interfaces;
using DotnetTemplate.EntityFramework.Shared.DbContexts;
using DotnetTemplate.EntityFramework.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DotnetTemplate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditController : ControllerBase
    {
        private readonly ILogger<AuditController> _logger;
        private readonly IAuditService _auditService;

        public AuditController(ILogger<AuditController> logger, IAuditService auditService)
        {
            _logger = logger;
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<List<AuditDto>> Get()
        {
            return await _auditService.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return Ok();
        }
    }
}

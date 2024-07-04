
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetTemplate.EntityFramework.Shared.Entities
{
    [Table("audits")]
    public class AuditDto
    {
        public int Id { get; set; }
        public string? Route { get; set; }
        public string? Ip { get; set; }
        public string? Method { get; set; }
        public string? Origin { get; set; }

    }
}

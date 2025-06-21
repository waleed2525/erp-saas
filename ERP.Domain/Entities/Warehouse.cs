using System;
using ERP.Domain.Common;
namespace ERP.Domain.Entities
{
    public class Warehouse : BaseAuditableEntity
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Location { get; set; } = default!;
    }
}

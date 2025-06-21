using System;
using ERP.Domain.Common;
namespace ERP.Domain.Entities
{
    public class Item : BaseAuditableEntity
    {
        public string SKU { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}

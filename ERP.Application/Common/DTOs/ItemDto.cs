using System;

namespace ERP.Application.Common.DTOs
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}

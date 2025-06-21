using System;
using System.Collections.Generic;

namespace ERP.Application.Common.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
        public IEnumerable<AccountDto>? ChildAccounts { get; set; }
    }
}

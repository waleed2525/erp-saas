using System;
using System.Collections.Generic;
using ERP.Domain.Common;
namespace ERP.Domain.Entities
{
    public class Account : BaseAuditableEntity
    {
        public string AccountNumber { get; set; } = default!;
        public string Name { get; set; } = default!;
        public Guid? ParentAccountId { get; set; }
        public Account? ParentAccount { get; set; }
        public ICollection<Account> ChildAccounts { get; set; } = new List<Account>();
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ERP.Application.Common.Interfaces;
using ERP.Domain.Entities;

namespace ERP.Application.Features.Financial.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IApplicationDbContext _db;

        public CreateAccountCommandHandler(IApplicationDbContext db) => _db = db;

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var entity = new Account
            {
                Id = Guid.NewGuid(),
                TenantId = Guid.NewGuid(), // In real case resolve from middleware
                AccountNumber = request.AccountNumber,
                Name = request.Name,
                ParentAccountId = request.ParentId
            };
            _db.Accounts.Add(entity);
            await _db.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}

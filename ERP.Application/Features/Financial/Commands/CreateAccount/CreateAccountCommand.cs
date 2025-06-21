using MediatR;
using System;

namespace ERP.Application.Features.Financial.Commands.CreateAccount
{
    public record CreateAccountCommand(string AccountNumber, string Name, Guid? ParentId) : IRequest<Guid>;
}

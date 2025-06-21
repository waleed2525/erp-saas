using MediatR;
using System.Collections.Generic;
using ERP.Application.Common.DTOs;

namespace ERP.Application.Features.Inventory.Queries.GetItems
{
    public record GetItemsQuery : IRequest<IEnumerable<ItemDto>>;
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ERP.Application.Common.DTOs;
using ERP.Application.Common.Interfaces;

namespace ERP.Application.Features.Inventory.Queries.GetItems
{
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<ItemDto>>
    {
        private readonly IApplicationDbContext _db;
        private readonly IMapper _mapper;

        public GetItemsQueryHandler(IApplicationDbContext db, IMapper mapper) =>
            (_db, _mapper) = (db, mapper);

        public async Task<IEnumerable<ItemDto>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
            => await _db.Items.ProjectTo<ItemDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}

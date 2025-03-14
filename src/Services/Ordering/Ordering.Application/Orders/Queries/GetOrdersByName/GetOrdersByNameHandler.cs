using Ordering.Application.Extensions;
using System.Collections.Generic;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        /// get order by name using dbContext
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(x => x.OrderName.Value.Contains(query.Name))
            .OrderBy(x => x.OrderName)
            .ToListAsync();

        var orderDtos = orders.ToOrderDtoList();

        /// return result
        return new GetOrdersByNameResult(orderDtos);
       
       
    }

}

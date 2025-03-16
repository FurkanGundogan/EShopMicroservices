using Ordering.Application.Extensions;
using System.Collections.Generic;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        /// get order by name using dbContext
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(x => x.CustomerId == CustomerId.Of(query.CustomerId))
            .OrderBy(x => x.OrderName.Value)
            .ToListAsync();

        var orderDtos = orders.ToOrderDtoList();

        /// return result
        return new GetOrdersByCustomerResult(orderDtos);
       
       
    }

}

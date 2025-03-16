using BuildingBlocks.Pagination;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders;


public class GetOrdersHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalCount = await dbContext.Orders.LongCountAsync();

        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .OrderBy(o => o.OrderName.Value)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .ToListAsync();

        var orderDtoList = orders.ToOrderDtoList();
        var paginatedResult = new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orderDtoList);

        return new GetOrdersResult(paginatedResult); 
    }
}

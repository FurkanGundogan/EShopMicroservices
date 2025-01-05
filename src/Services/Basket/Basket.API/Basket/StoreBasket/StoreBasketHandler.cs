using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Card can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("Username is required");
        }
    }

    public class StoreBasketCommandHandler
        (IBasketRepository repository,
         DiscountProtoService.DiscountProtoServiceClient discountProto) 
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            // communicate with discount grpc
            await DeductDiscount(command.Cart, cancellationToken);

            /// store basket in db using marten and redis cache
            await repository.StoreBasket(command.Cart, cancellationToken);

            return new StoreBasketResult(command.Cart.UserName);
        }


        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellation)
        {
            // communicate with discount grpc
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest()
                {
                    ProductName = item.ProductName
                });
                item.Price -= coupon.Amount;
            }
        }
    }


}

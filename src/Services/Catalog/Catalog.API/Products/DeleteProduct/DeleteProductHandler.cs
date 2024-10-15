
using FluentValidation;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductComand(Guid Id) : ICommand<DeleteProductResult>;

    public record DeleteProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductComand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product id is required");
        }
    }

    internal class DeleteProductHandler
        (IDocumentSession session, ILogger<DeleteProductHandler> logger)
        : ICommandHandler<DeleteProductComand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductComand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductHandler.Handle called whith {@Command}", command);

            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);

        }
    }
}

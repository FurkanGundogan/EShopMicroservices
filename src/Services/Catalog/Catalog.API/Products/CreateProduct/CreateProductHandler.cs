namespace Catalog.API.Products.CreateProduct
{

    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);


    ///  IDocumentSession is abstraction of db operations

    internal class CreateProductCommandHandler (IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Business login to create product


            // create product entity from command

            var product = new Product {
                 Name = command.Name,
                 Category = command.Category,
                 Description = command.Description, 
                 ImageFile = command.ImageFile,
                 Price = command.Price
            };

            // save to db
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            // return result
            return new CreateProductResult(product.Id);


            
        }
    }
}

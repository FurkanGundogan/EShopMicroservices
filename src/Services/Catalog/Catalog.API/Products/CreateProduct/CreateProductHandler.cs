using BuildingBlocks.CQRS;
using Catalog.API.Models;


namespace Catalog.API.Products.CreateProduct
{


    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        :ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
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

            // return result
            return new CreateProductResult(Guid.NewGuid());


            
        }
    }
}

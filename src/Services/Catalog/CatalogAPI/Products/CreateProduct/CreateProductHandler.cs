namespace CatalogAPI.Products.CreateProduct
{

    public record CreateProductCommand(string name, List<string> category, string description, string imageFile, decimal price) 
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand> 
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Nome é obrigatório");
            RuleFor(x => x.category).NotEmpty().WithMessage("Categoria é obrigatório");
            RuleFor(x => x.imageFile).NotEmpty().WithMessage("Image File é obrigatório");
            RuleFor(x => x.price).GreaterThan(0).WithMessage("Preço precisa ser maior que 0");
        }
    };

    internal class CreateProductCommandHandler 
        (IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            Product product = new Product()
            {
                Name = command.name,
                Category = command.category,
                Description = command.description,
                ImageFile = command.imageFile,
                Price = command.price
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}

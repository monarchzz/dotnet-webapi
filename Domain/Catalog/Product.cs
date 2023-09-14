namespace Domain.Catalog;

public class Product : IAggregateRoot
{
    public Product(string name, string description)
    {
        Name = name;
        Description = Description;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public Brand Brand { get; set; } = default!;
    public Guid BrandId { get; set; }
}
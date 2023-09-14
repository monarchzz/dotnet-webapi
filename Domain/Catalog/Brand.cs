namespace Domain.Catalog;

public class Brand : IAggregateRoot
{
    public Brand(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public Guid Id { get; set; }
    
    public string Name { get; private set; }

    public string? Description { get; private set; }

    public ICollection<Product> Products { get; set; } = default!;
}
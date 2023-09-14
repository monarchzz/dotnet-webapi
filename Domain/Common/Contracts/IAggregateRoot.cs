namespace Domain.Common.Contracts;

public interface IAggregateRoot : IEntity
{
    public Guid Id { get; set; }
}
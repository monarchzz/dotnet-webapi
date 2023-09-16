namespace Application.Common.Models;

public class BaseDto : IDto
{
    public BaseDto(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
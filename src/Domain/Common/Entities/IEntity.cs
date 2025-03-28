namespace Domain.Common.Entities;

public interface IEntity<out TId>
{
    TId Id { get; }
}

namespace HIS.Data.Base.Interfaces.Models
{
  public interface IEntity<out T>
  {
    T Id { get; }
  }
}

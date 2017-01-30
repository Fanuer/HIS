namespace HIS.Bot.WebApi.Data.ViewModels.Interfaces
{
  public interface IEntity<out T>
  {
    T Id { get; }
  }
}

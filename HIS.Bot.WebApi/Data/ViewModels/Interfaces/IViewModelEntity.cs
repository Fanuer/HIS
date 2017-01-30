namespace HIS.Bot.WebApi.Data.ViewModels.Interfaces
{
    public interface IViewModelEntity<out TKey> : IEntity<TKey>
    {
        string Url { get; }
    }
}

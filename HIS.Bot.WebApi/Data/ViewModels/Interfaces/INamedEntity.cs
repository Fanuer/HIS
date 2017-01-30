namespace HIS.Bot.WebApi.Data.ViewModels.Interfaces
{
    public interface INamedEntity<out TKey>:IEntity<TKey>
    {
        string Name { get; set; }
    }
}

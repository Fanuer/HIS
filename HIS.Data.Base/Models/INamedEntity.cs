namespace HIS.Data.Base.Interfaces.Models
{
    public interface INamedEntity<out TKey>: IEntity<TKey>
    {
        string Name { get; set; }
    }
}

namespace HIS.Data.Base.Interfaces.Models
{
    public interface IRestEntity<TKey:INamedEntity<TKey>
    {
        string Url { get; set; }
    }
}

namespace HIS.Data.Base.Interfaces.Models
{
    public interface IFuzzyEntry
    {
        string SearchQuery { get; set; }
        IEntity<int> Entity { get; set; }
    }
}

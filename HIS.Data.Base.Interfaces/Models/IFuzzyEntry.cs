using System;

namespace HIS.Data.Base.Interfaces.Models
{
    public interface IFuzzyEntry<TIdProperty>:IEquatable<IFuzzyEntry<TIdProperty>>
    {
        string SearchQuery { get; set; }
        TIdProperty Id { get; set; }
        string Type { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces.Models
{
    public interface IViewModelEntity<out TKey> : IEntity<TKey>
    {
        string Url { get; }
    }
}

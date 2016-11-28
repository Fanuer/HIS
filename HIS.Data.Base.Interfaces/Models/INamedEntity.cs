using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces.Models
{
    public interface INamedEntity<out TKey>:IEntity<TKey>
    {
        string Name { get; set; }
    }
}

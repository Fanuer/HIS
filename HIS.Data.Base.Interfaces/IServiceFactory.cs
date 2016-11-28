using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces
{
    /// <summary>
    /// Interface of a Factory to create service Instances
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IServiceFactory<T> where T : class
    {
        T Build();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Exceptions
{
    public class DataObjectNotFoundException:ArgumentException
    {
        public DataObjectNotFoundException(string message = null)
            :base(message ?? "Data object not found")
        {
            
        }
    }
}

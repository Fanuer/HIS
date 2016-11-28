using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Exceptions
{
    public class IdsNotIdenticalException : ArgumentException
    {
        public IdsNotIdenticalException(string message = null)
            : base(message ?? "The given id must match the model's id.")
        {
        }
    }
}

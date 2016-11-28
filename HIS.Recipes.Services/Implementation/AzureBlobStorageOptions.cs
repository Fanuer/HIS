using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Services.Implementation
{
    public class AzureBlobStorageOptions
    {
        public string BlobStorageConnectionString { get; set; }
        public string BaseContainer { get; set; }

    }
}

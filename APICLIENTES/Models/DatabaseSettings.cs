using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APICLIENTES.Models
{
    public class DatabaseSettings:IDatabaseSettings
    {
        public string CollectionName { get; set; }
        public string CollectionName1 { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IDatabaseSettings
    {
        string CollectionName { get; set; }
        string CollectionName1 { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}


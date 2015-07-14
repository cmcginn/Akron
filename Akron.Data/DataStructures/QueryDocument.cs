using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Akron.Data.DataStructures
{
    public class QueryDocument
    {
        public string CollectionName { get; set; }
        public string DataSourceLocation { get; set; }
        public string DataSource { get; set; }
        public BsonDocument Project { get; set; }
        public MatchDefinition Match { get; set; }
        public GroupDefinition Group { get; set; }
      
    }
}

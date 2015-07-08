using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{
    public class QueryDocument
    {
        public string CollectionName { get; set; }

        public GroupDefinition Group { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akron.Data.DataStructures;

namespace Akron.Data
{
    public class QueryBuilder
    {
        //public string CollectionName { get; set; }
        //public string DataSourceLocation { get; set; }
        //public string DataSource { get; set; }

       // private List<QueryField> _Slicers;

        private List<QueryField> _AvailableQueryFields;

        //private List<FactDefinition> _Facts;

        //public GroupDefinition GroupDefinition { get; set; }
        public List<QueryField> AvailableQueryFields
        {
            get { return _AvailableQueryFields ?? (_AvailableQueryFields = new List<QueryField>()); }
            set { _AvailableQueryFields = value; }
        }

      
    }
}

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
        private List<QueryField> _AvailableQueryFields;

        public List<QueryField> AvailableQueryFields
        {
            get { return _AvailableQueryFields ?? (_AvailableQueryFields = new List<QueryField>()); }
            set { _AvailableQueryFields = value; }
        }
    }
}

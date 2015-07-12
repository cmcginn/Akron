using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{
    public class QueryField
    {
        public DataColumnMetadata Column { get; set; }

        public List<QueryFieldValue> AvailableValues
        {
            get { return _AvailableValues ?? (_AvailableValues = new List<QueryFieldValue>()); }
            set { _AvailableValues = value; }
        }

        public QueryFieldValue SelectedValue { get; set; }

        private List<QueryFieldValue> _AvailableValues;

    }
}

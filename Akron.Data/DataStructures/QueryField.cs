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

        public List<string> AvailableValues
        {
            get { return _AvailableValues ?? (_AvailableValues = new List<string>()); }
            set { _AvailableValues = value; }
        }

        private List<string> _AvailableValues;

    }
}

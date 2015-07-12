using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{
    public class FilterDataColumnMetadata : DataColumnMetadata
    {
        private List<DataColumnMetadata> _FilterDependencyColumns;

        public List<DataColumnMetadata> FilterDependencyColumns
        {
            get { return _FilterDependencyColumns ?? (_FilterDependencyColumns = new List<DataColumnMetadata>()); }
            set { _FilterDependencyColumns = value; }
        }
    }
}

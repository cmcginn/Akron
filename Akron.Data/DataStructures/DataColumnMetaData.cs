using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{

    public class DataColumnMetadata
    {
        public ColumnDataTypes DataType { get; set; }
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }
        public bool DisplayOnly { get; set; }
        public string DisplayName { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{
    public class DataCollectionMetadata
    {
        public string DisplayName { get; set; }
        public string DataSourceName { get; set; }
        public string CollectionName { get; set; }

        private List<DataColumnMetadata> _Filters;

        public List<DataColumnMetadata> Filters
        {
            get { return _Filters ?? (_Filters = new List<DataColumnMetadata>()); }
            set { _Filters = value; }
        }

        private List<DataColumnMetadata> _Measures;

        public List<DataColumnMetadata> Measures
        {
            get { return _Measures ?? (_Measures = new List<DataColumnMetadata>()); }
            set { _Measures = value; }
        }

        private List<DataColumnMetadata> _Dimensions;

        public List<DataColumnMetadata> Dimensions
        {
            get { return _Dimensions ?? (_Dimensions = new List<DataColumnMetadata>()); }
            set { _Dimensions = value; }
        }

        private List<DataColumnMetadata> _Columns;

        public List<DataColumnMetadata> Columns
        {
            get { return _Columns ?? (_Columns = new List<DataColumnMetadata>()); }
            set { _Columns = value; }
        }
    }
}

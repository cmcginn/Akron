﻿using System;
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

        private List<FilterDataColumnMetadata> _Filters;

        public List<FilterDataColumnMetadata> Filters
        {
            get { return _Filters ?? (_Filters = new List<FilterDataColumnMetadata>()); }
            set { _Filters = value; }
        }

        private List<MeasureDataColumnMetadata> _Measures;

        public List<MeasureDataColumnMetadata> Measures
        {
            get { return _Measures ?? (_Measures = new List<MeasureDataColumnMetadata>()); }
            set { _Measures = value; }
        }

        private List<DimensionColumnMetadata> _Dimensions;

        public List<DimensionColumnMetadata> Dimensions
        {
            get { return _Dimensions ?? (_Dimensions = new List<DimensionColumnMetadata>()); }
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

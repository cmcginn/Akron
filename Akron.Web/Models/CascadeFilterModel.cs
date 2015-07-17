using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Akron.Data.DataStructures;

namespace Akron.Web.Models
{
    public class CascadeFilterModel
    {
        public string ParentColumnName { get; set; }
        public string ColumnName { get; set; }

        private List<FilterValue> _ParentColumnValues;
        public List<FilterValue> ParentColumnValues
        {
            get { return _ParentColumnValues ?? (_ParentColumnValues = new List<FilterValue>()); }
            set { _ParentColumnValues = value; }
        }
    }
}
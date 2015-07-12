using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akron.Web.Models
{
    public class CascadeFilterModel
    {
        public string ParentColumnName { get; set; }
        public string ColumnName { get; set; }
        public string ParentColumnValue { get; set; }
    }
}
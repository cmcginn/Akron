using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akron.Web.Models
{
    public class SeriesXY
    {
        public object Key { get; set; }

        public List<SeriesValue> Values
        {
            get { return _Values ?? (_Values = new List<SeriesValue>()); }
            set { _Values = value; }
        }

        private List<SeriesValue> _Values;

    }

    public class SeriesValue
    {
        public object Series { get; set; }
        public object Value { get; set; }
    }
}
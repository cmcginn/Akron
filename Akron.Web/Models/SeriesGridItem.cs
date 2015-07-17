using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akron.Web.Models
{
    public class SeriesGridItem
    {
        public string Key { get; set; }
        public List<KeyValuePair<string, double>> Value { get; set; }
    }
}
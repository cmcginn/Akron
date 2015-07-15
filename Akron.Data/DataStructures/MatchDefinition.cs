using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{
    public class MatchDefinition
    {
        private List<FilterDefinition> _Filters;

        public List<FilterDefinition> Filters
        {
            get { return _Filters ?? (_Filters = new List<FilterDefinition>()); }
            set { _Filters = value; }
        }
    }
}

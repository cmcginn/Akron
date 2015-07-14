using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{
    public class MatchDefinition
    {
        private List<QueryField> _Filters;

        public List<QueryField> Filters
        {
            get { return _Filters ?? (_Filters = new List<QueryField>()); }
            set { _Filters = value; }
        }
    }
}

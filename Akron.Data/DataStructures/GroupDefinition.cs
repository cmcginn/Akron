using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akron.Data.DataStructures
{
    public class GroupDefinition
    {
        //Becomes grouping, used for XAxis
        List<string> _Slicers;
        public List<string> Slicers
        {
            get { return _Slicers ?? (_Slicers = new List<string>()); }
            set { _Slicers = value; }
        }
        //Becomes Y Axis Values
        List<FactDefinition> _Facts;
        public List<FactDefinition> Facts
        {
            get { return _Facts ?? (_Facts = new List<FactDefinition>()); }
            set { _Facts = value; }
        }
    }
}

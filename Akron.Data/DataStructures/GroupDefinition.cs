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
        List<DimensionDefinition> _Dimensions;
        public List<DimensionDefinition> Dimensions
        {
            get { return _Dimensions ?? (_Dimensions = new List<DimensionDefinition>()); }
            set { _Dimensions = value; }
        }
        //Becomes Y Axis Values
        List<MeasureDefinition> _Measures;
        public List<MeasureDefinition> Measures
        {
            get { return _Measures ?? (_Measures = new List<MeasureDefinition>()); }
            set { _Measures = value; }
        }
    }
}

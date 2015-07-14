using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akron.Data.DataStructures;

namespace Akron.Data
{
    public class QueryBuilder
    {
     

       private List<QueryField> _AvailableSlicers;

        private List<QueryField> _AvailableQueryFields;

        private List<MeasureDefinition> _AvailableMeasures;

        public List<QueryField> AvailableQueryFields
        {
            get { return _AvailableQueryFields ?? (_AvailableQueryFields = new List<QueryField>()); }
            set { _AvailableQueryFields = value; }
        }

        public List<QueryField> AvailableSlicers
        {
            get { return _AvailableSlicers ?? (_AvailableSlicers = new List<QueryField>()); }
            set { _AvailableSlicers = value; }
        }

        public List<MeasureDefinition> AvailableMeasures
        {
            get { return _AvailableMeasures ?? (_AvailableMeasures = new List<MeasureDefinition>()); }
            set { _AvailableMeasures = value; }
        }
    }
}

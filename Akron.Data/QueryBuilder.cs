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
     

        private List<DimensionDefinition> _AvailableSlicers;

        private List<DimensionDefinition> _SelectedSlicers;

        private List<FilterDefinition> _AvailableFilters;

        private List<FilterDefinition> _SelectedFilters;

        private List<MeasureDefinition> _AvailableMeasures;
        private List<MeasureDefinition> _SelectedMeasures;

        public List<FilterDefinition> AvailableFilters
        {
            get { return _AvailableFilters ?? (_AvailableFilters = new List<FilterDefinition>()); }
            set { _AvailableFilters = value; }
        }
        public List<FilterDefinition> SelectedFilters
        {
            get { return _SelectedFilters ?? (_SelectedFilters = new List<FilterDefinition>()); }
            set { _SelectedFilters = value; }
        }
        public List<DimensionDefinition> AvailableSlicers
        {
            get { return _AvailableSlicers ?? (_AvailableSlicers = new List<DimensionDefinition>()); }
            set { _AvailableSlicers = value; }
        }
        public List<DimensionDefinition> SelectedSlicers
        {
            get { return _SelectedSlicers ?? (_SelectedSlicers = new List<DimensionDefinition>()); }
            set { _SelectedSlicers = value; }
        }
        public List<MeasureDefinition> AvailableMeasures
        {
            get { return _AvailableMeasures ?? (_AvailableMeasures = new List<MeasureDefinition>()); }
            set { _AvailableMeasures = value; }
        }
        public List<MeasureDefinition> SelectedMeasures
        {
            get { return _SelectedMeasures ?? (_SelectedMeasures = new List<MeasureDefinition>()); }
            set { _SelectedMeasures = value; }
        }
      
    }
}

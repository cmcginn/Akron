using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Akron.Data;
using Akron.Data.DataStructures;
using Akron.Web.Models;
using MongoDB.Bson;
using Akron.Data.Helpers;
namespace Akron.Web.Services
{
    public class AkronService
    {
        static string GetDimensionLabel(string collectionType)
        {
            string result = "";
            switch (collectionType)
            {
                case "OrgType":
                    result = "Organization Types";
                    break;
                case "JobFamily":
                    result = "Job Families";
                    break;
                case "GeoGroup":
                    result = "Geo Groups";
                    break;
                default: break;
            }
            return result;
        }
        public AkronModel GetModel(string collectionType)
        {
            var result = new AkronModel();
            result.DimensionLabel = GetDimensionLabel(collectionType);
            var ds = new DataService();
            var tasks = new Task[4];
            var basePayTask = new Task(() =>
            {
                result.BasePayByYearAndDimension = ds.BasePayByYearAndDimension(String.Format("basePayByYear{0}",collectionType));
            });
            tasks[0] = basePayTask;
            tasks[0].Start();

            var countTask = new Task(() =>
            {
                result.RecordCount = ds.GetTotalCount();
            });

            tasks[1] = countTask;
            tasks[1].Start();

            var averageTask = new Task(() =>
            {
                result.TotalAverage = (int) ds.Average();
            });

            tasks[2] = averageTask;
            tasks[2].Start();

            var countByOrgTypeTask = new Task(() =>
            {
                result.CountByDimension = ds.CountByDimension(String.Format("countBy{0}",collectionType));

            });
            tasks[3] = countByOrgTypeTask;
            tasks[3].Start();

            Task.WaitAll(tasks);

            return result;
        }

        public QueryBuilder GetQueryBuilder(string collectionName)
        {
            //var result = new QueryBuilder();
            var service = new DataService();
            var result = service.GetQueryBuilder(collectionName);
            result.SelectedFilters.Add(result.AvailableFilters.First());
            //TODO make Selectable
           // var year = new DimensionDefinition { Column = new DataColumnMetadata { ColumnName = "Year" }, IsDefault=true};
            //var jobFamily = new DimensionDefinition {Column = new DataColumnMetadata {ColumnName = "Job_Family"}};
            
            //var basePay = new QueryField { Column = new DataColumnMetadata { ColumnName = "Base_Pay" }, SelectedValue = new FilterValue { Key = "Base_Pay", Value = "Base_Pay" } };
            //var basePayMeasure = new MeasureDefinition { Column = new DataColumnMetadata { ColumnName = "Base_Pay" }, IsDefault=true, Operation = AggregateOperations.Average };
           //default x
            //result.AvailableSlicers.Add(year);
            //result.AvailableSlicers.Add(jobFamily);
            
            //result.AvailableMeasures.Add(basePayMeasure);
            return result;
        }

        public List<BsonDocument> GetSeriesGrid(QueryBuilder builder)
        {
            builder.SelectedSlicers.Insert(0, builder.AvailableSlicers.Single(x => x.IsDefault));
            //add default org type
            if (builder.SelectedSlicers.Count == 1)
                builder.SelectedSlicers.Add(
                    builder.AvailableSlicers.SingleOrDefault(x => x.Column.ColumnName == "org_type"));


            builder.SelectedMeasures = new List<MeasureDefinition> { builder.AvailableMeasures.Single(x => x.IsDefault) };
            var qd = builder.ToQueryDocument();
            qd.CollectionName = "incumbent";
            qd.DataSource = "hra";
            qd.DataSourceLocation = "mongodb://localhost:27017";
            var service = new DataService();

            var result = service.GetData(qd).ToList();
    
            return result;
        }
        public List<BsonDocument> GetSeries(QueryBuilder builder)
        {
            //year column is default;

            builder.SelectedSlicers.Insert(0, builder.AvailableSlicers.Single(x => x.IsDefault));
            //add default org type
            if (builder.SelectedSlicers.Count == 1)
                builder.SelectedSlicers.Add(
                    builder.AvailableSlicers.SingleOrDefault(x => x.Column.ColumnName == "org_type"));


            builder.SelectedMeasures = new List<MeasureDefinition> {builder.AvailableMeasures.Single(x => x.IsDefault)};
            var qd = builder.ToSeriesQueryDocument();
            qd.CollectionName = "incumbent";
            qd.DataSource = "hra";
            qd.DataSourceLocation = "mongodb://localhost:27017";
            var service = new DataService();

            var result = service.GetData(qd);
            return result;
        }
        public List<FilterValue> GetFilteredQueryFieldValues(CascadeFilterModel model)
        {
            var service = new DataService();
            var result = service.GetFilteredQueryFields(model.ParentColumnName, model.ColumnName,
                model.ParentColumnValues.Select(x=>x.Value).ToList());
            return result;
        }

        public List<BsonDocument> QueryData(QueryBuilder builder)
        {
           
            var qd = builder.ToQueryDocument();
            qd.CollectionName = "incumbent";
            qd.DataSource = "hra";
            qd.DataSourceLocation = "mongodb://localhost:27017";
            var service = new DataService();
            return service.GetData(qd);
        }
    }
}
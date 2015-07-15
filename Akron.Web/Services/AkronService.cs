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
            //TODO make Selectable
            var year = new QueryField { Column = new DataColumnMetadata { ColumnName = "Year" }, SelectedValue = new QueryFieldValue { Key = "Year", Value = "Year" } };
            var jobFamily = new QueryField {Column = new DataColumnMetadata {ColumnName = "Job_Family"}, SelectedValue=new QueryFieldValue{ Key="Job_Family", Value="Job_Family"}};
            
            var basePay = new QueryField { Column = new DataColumnMetadata { ColumnName = "Base_Pay" }, SelectedValue = new QueryFieldValue { Key = "Base_Pay", Value = "Base_Pay" } };
            var basePayMeasure = new MeasureDefinition {QueryField = basePay, Operation = AggregateOperations.Average};
           //default x
            result.AvailableSlicers.Add(year);
            result.AvailableSlicers.Add(jobFamily);
            
            result.AvailableMeasures.Add(basePayMeasure);
            return result;
        }

        public List<SeriesXY> GetSeries(QueryBuilder builder)
        {
            var qd = builder.ToSeriesQueryDocument();
            qd.CollectionName = "incumbent";
            qd.DataSource = "hra";
            qd.DataSourceLocation = "mongodb://localhost:27017";
            var service = new DataService();
            var result = new List<SeriesXY>();
            service.GetData(qd).ToList().ForEach(x =>
            {
                var item = new SeriesXY();
                item.Key = x["key"];
                var values = x["f0"] as BsonArray;
                item.Values = new List<SeriesValue>();
                for (var i = 0; i < values.Count; i++)
                {
                    var seriesValue = new SeriesValue();
                    seriesValue.Series = x["f0"][i]["s1"];
                    seriesValue.Value = x["f0"][i]["f0"];
                    item.Values.Add(seriesValue);
                }
                result.Add(item);
            });
            return result;
        }
        public List<QueryFieldValue> GetFilteredQueryFieldValues(CascadeFilterModel model)
        {
            var service = new DataService();
            var result = service.GetFilteredQueryFields(model.ParentColumnName, model.ColumnName,
                model.ParentColumnValue);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Akron.Data;
using Akron.Web.Models;

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
            return result;
        }
    }
}
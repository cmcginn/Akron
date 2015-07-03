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
        public AkronModel GetModel()
        {
            var result = new AkronModel();
            var ds = new DataService();
            var tasks = new Task[4];
            var basePayTask = new Task(() =>
            {
                result.BasePayByYearAndOrgType = ds.BasePayByYearOrgType();
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
                result.CountByOrgType = ds.CountByOrgType();

            });
            tasks[3] = countByOrgTypeTask;
            tasks[3].Start();

            Task.WaitAll(tasks);

            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Akron.Data;
using Akron.Data.DataStructures;
using Akron.Web.Services;
using MongoDB.Bson;
using Akron.Data.Helpers;
namespace Akron.Web.ApiControllers
{
    public class IncumbentController : ApiController
    {
       private AkronService svc = new AkronService();
        // GET: api/Incumbent
       public IEnumerable<BsonDocument> Get()
       {

           var svc = new Akron.Data.DataService();
           var queryDoc = new QueryDocument();
           var queryGroup = new GroupDefinition();

           var qb = new QueryBuilder();

           qb.SelectedSlicers.Add(new DimensionDefinition
           {
               Column = new DataColumnMetadata { ColumnName = "Year" },
               IsDefault = true
           });
           qb.SelectedSlicers.Add(new DimensionDefinition
           {
               Column = new DataColumnMetadata { ColumnName = "org_type" }

           });
           qb.SelectedMeasures = new List<MeasureDefinition>
            {
                new MeasureDefinition
                {
                    Column = new DataColumnMetadata {ColumnName = "Base_Pay"},
                    IsDefault = true,
                    Operation = AggregateOperations.Average
                }
            };
           var qd = qb.ToQueryDocument();
           qd.CollectionName = "incumbent";
           qd.DataSource = "hra";
           qd.DataSourceLocation = "mongodb://localhost:27017";
           var result = svc.GetData(qd);
           return result;

       }
       //public IEnumerable<BsonDocument> Get()
       //{

       //    var svc = new Akron.Data.DataService();
       //    var queryDoc = new QueryDocument();
       //    var queryGroup = new GroupDefinition();

       //    var qb = new QueryBuilder();

       //    qb.SelectedSlicers.Add(new DimensionDefinition
       //    {
       //        Column = new DataColumnMetadata { ColumnName = "Year" },
       //        IsDefault = true
       //    });
       //    qb.SelectedSlicers.Add(new DimensionDefinition
       //    {
       //        Column = new DataColumnMetadata { ColumnName = "org_type" }

       //    });
       //    qb.SelectedMeasures = new List<MeasureDefinition>
       //    {
       //        new MeasureDefinition
       //        {
       //            Column = new DataColumnMetadata {ColumnName = "Base_Pay"},
       //            IsDefault = true,
       //            Operation = AggregateOperations.Average
       //        }
       //    };
       //    var qd = qb.ToQueryDocument();
       //    qd.CollectionName = "incumbent";
       //    qd.DataSource = "hra";
       //    qd.DataSourceLocation = "mongodb://localhost:27017";
       //    var result = new List<BsonDocument>();
       //    //var grouped = svc.GetData(qd).GroupBy(x=>x["s0"])
       //    return result;

       //}
        // GET: api/Incumbent/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Incumbent
        public void Post(QueryBuilder value)
        {
            var m = value;
            var x = "Y";
        }

        // PUT: api/Incumbent/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Incumbent/5
        public void Delete(int id)
        {
        }
    }
}

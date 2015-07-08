using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Akron.Data.DataStructures;
using Akron.Web.Services;
using MongoDB.Bson;

namespace Akron.Web.ApiControllers
{
    public class IncumbentController : ApiController
    {
       // private AkronService svc = new AkronService();
        // GET: api/Incumbent
        public IEnumerable<BsonDocument> Get()
        {
            var svc = new Akron.Data.DataService();
            var queryDoc = new QueryDocument();
            var queryGroup = new GroupDefinition();
            queryGroup.Slicers.Add("Year");
            queryGroup.Slicers.Add("org_type");
            queryGroup.Facts.Add(new FactDefinition {Name = "Base_Pay", Operation = AggregateOperations.Average});
            queryDoc.CollectionName = "incumbent";
            queryDoc.Group = queryGroup;
            var result = svc.GetData(queryDoc);
            return result;
        }

        // GET: api/Incumbent/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Incumbent
        public void Post([FromBody]string value)
        {
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

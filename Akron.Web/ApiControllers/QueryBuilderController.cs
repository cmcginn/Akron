using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Akron.Data;
using Akron.Web.Services;
using MongoDB.Bson;

namespace Akron.Web.ApiControllers
{
    public class QueryBuilderController : ApiController
    {
        private AkronService svc = new AkronService();
        // GET: api/QueryBuilder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/QueryBuilder/5
        public Akron.Data.QueryBuilder Get(string id)
        {
            return svc.GetQueryBuilder(id);
        }

        // POST: api/QueryBuilder
        public List<BsonDocument> Post(QueryBuilder value)
        {
            var result = svc.QueryData(value);
            return result;
        }

        // PUT: api/QueryBuilder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/QueryBuilder/5
        public void Delete(int id)
        {
        }
    }
}

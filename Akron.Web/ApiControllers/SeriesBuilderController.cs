using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Akron.Data;
using Akron.Web.Models;
using Akron.Web.Services;

namespace Akron.Web.ApiControllers
{
    public class SeriesBuilderController : ApiController
    {
        private AkronService svc = new AkronService();
        // GET: api/SeriesBuilder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SeriesBuilder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SeriesBuilder
        public List<SeriesXY> Post(QueryBuilder value)
        {
            var result = svc.GetSeries(value);
            return result;
        }

        // PUT: api/SeriesBuilder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SeriesBuilder/5
        public void Delete(int id)
        {
        }
    }
}

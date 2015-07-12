using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Akron.Web.Models;

namespace Akron.Web.ApiControllers
{
    public class FilterDataColumnController : ApiController
    {
        // GET: api/FilterDataColumn
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/FilterDataColumn/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/FilterDataColumn
        public void Post(CascadeFilterModel value)
        {
            var x = value;
        }

        // PUT: api/FilterDataColumn/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FilterDataColumn/5
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Akron.Web.Models;
using Akron.Web.Services;

namespace Akron.Web.ApiControllers
{
    public class IncumbentController : ApiController
    {
        private AkronService svc = new AkronService();
        // GET: api/Incumbent
        public AkronModel Get()
        {
            return svc.GetModel("OrgType");
        }

        // GET: api/Incumbent/5
        public AkronModel Get(string id)
        {
            return svc.GetModel(id);
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

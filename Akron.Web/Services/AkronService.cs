using System;
using System.Collections.Generic;
using System.Linq;
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
            result.BasePayByYearAndOrgType = ds.GetByOrgType();
            return result;
        }
    }
}
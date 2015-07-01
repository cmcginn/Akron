using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Akron.Web.Models
{
    public class AkronModel
    {
        public List<BsonDocument> BasePayByYearAndOrgType { get; set; }
        public int RecordCount { get; set; }
    }
}
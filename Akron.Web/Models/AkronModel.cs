using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Akron.Web.Models
{
    public class AkronModel
    {
        public List<BsonDocument> BasePayByYearAndDimension { get; set; }
        public List<BsonDocument> CountByDimension { get; set; }
        public long RecordCount { get; set; }
        public string DimensionLabel { get; set; }
        public int TotalAverage { get; set; }
    }
}
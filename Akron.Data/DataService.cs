using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Akron.Data
{
    public class DataService
    {
        public List<BsonDocument> GetByOrgType()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<BsonDocument>("incumbentMap");

            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        {
                            "_id", new BsonDocument
                            {
                                {"orgType", "$value.orgType"},
                                {"year", "$value.year"},
                            }

                        },
                        {
                            "averageBasePay", new BsonDocument
                            {
                                {"$avg", "$value.basePay"}
                            }
                        }
                    }

                }
            };
            var pipeline = new[]
            {
                group
            };
            var result = items.AggregateAsync<BsonDocument>(pipeline).Result;

            var myList = result.ToListAsync<BsonDocument>().Result;
            return myList;
        }
    }
}

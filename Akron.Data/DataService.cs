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
        public List<BsonDocument> BasePayByYearOrgType()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<BsonDocument>("basePayByYearOrgType");

            return items.Find(new BsonDocument()).ToListAsync().Result;
        }

        public List<BsonDocument> CountByOrgType()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<BsonDocument>("countByOrgType");

            return items.Find(new BsonDocument()).ToListAsync().Result;
        }
        //public List<BsonDocument> GetByOrgType()
        //{
        //    var client = new MongoClient("mongodb://localhost:27017");
        //    var db = client.GetDatabase("hra");
        //    var items = db.GetCollection<BsonDocument>("incumbentMap");

        //    var group = new BsonDocument
        //    {
        //        {
        //            "$group", new BsonDocument
        //            {
        //                {
        //                    "_id", new BsonDocument
        //                    {
        //                        {"orgType", "$value.orgType"},
        //                        {"year", "$value.year"},
        //                    }

        //                },
        //                {
        //                    "averageBasePay", new BsonDocument
        //                    {
        //                        {"$avg", "$value.basePay"}
        //                    }
        //                }
        //            }

        //        }
        //    };
        //    var pipeline = new[]
        //    {
        //        group
        //    };
        //    var result = items.AggregateAsync<BsonDocument>(pipeline).Result;

        //    var myList = result.ToListAsync<BsonDocument>().Result;
        //    return myList;
        //}

        public double Average()
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
            return (double) myList.Single()["averageBasePay"];
        }
        public long GetTotalCount()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<BsonDocument>("incumbent");

            var result = items.CountAsync(new BsonDocument()).Result;
            return result;
        }
    }
}

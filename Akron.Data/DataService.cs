using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akron.Data.DataStructures;
using MongoDB.Bson;
using MongoDB.Driver;
using Akron.Data.Helpers;
namespace Akron.Data
{
    public class DataService
    {
        public List<BsonDocument> BasePayByYearAndDimension(string collectionName)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<BsonDocument>(collectionName);

            return items.Find(new BsonDocument()).ToListAsync().Result;
        }

        public List<BsonDocument> CountByDimension(string collectionName)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<BsonDocument>(collectionName);

            return items.Find(new BsonDocument()).ToListAsync().Result;
        }

        public List<BsonDocument> GetData(QueryDocument query)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<BsonDocument>(query.CollectionName);

            var g  = query.Group.ToGroup();
            var pipeline = new[]
                {
                    g
                };
            var docs = items.AggregateAsync<BsonDocument>(pipeline).Result;
            var result = docs.ToListAsync<BsonDocument>().Result;
            return result;

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

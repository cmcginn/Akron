using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akron.Data.DataStructures;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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

        //public List<BsonDocument> QueryData(QueryBuilder query)
        //{
        //    var doc = query.ToQueryDocument();
        //    return GetData(doc);
        //}

        public List<BsonDocument> GetData(QueryDocument query)
        {
            var client = new MongoClient(query.DataSourceLocation);
            var db = client.GetDatabase(query.DataSource);
            var items = db.GetCollection<BsonDocument>(query.CollectionName);


            var m = query.Match.ToMatchDocument();
            var g  = query.Group.ToGroupDocument();
            var p = query.Project;
            
            var pipeline = new[]
                {
                    m,
                    g,
                    p
                };
            var docs = items.AggregateAsync<BsonDocument>(pipeline).Result;
            var result = docs.ToListAsync<BsonDocument>().Result;
            return result;

        }

        public QueryBuilder GetQueryBuilder(string collectionName)
        {
            var result = new QueryBuilder();
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<DataCollectionMetadata>("collectionMetadata");
            var collectionItems = db.GetCollection<BsonDocument>("incumbent");

            FilterDefinition<DataCollectionMetadata> filter = new BsonDocument("_id", "incumbent");
            var md = items.Find<DataCollectionMetadata>(filter);
            var metadata = md.SingleAsync().Result;
            var tasks = new List<Task<IAsyncCursor<string>>>();
            metadata.Filters.ForEach(f =>
            {
                var queryField = new QueryField();

                queryField.Column = f;
                if (!f.FilterDependencyColumns.Any())
                {
                    queryField.AvailableValues.Add(new QueryFieldValue {Key = "All"});
                    FieldDefinition<BsonDocument, string> field = f.ColumnName;

                    var dd = Task<IAsyncCursor<string>>.Factory.StartNew(() =>
                    {
                        var t = collectionItems.DistinctAsync<string>(field, new BsonDocument());
                        t.GetAwaiter().OnCompleted(() =>
                        {
                            t.Result.ForEachAsync((z) =>
                            {
                                queryField.AvailableValues.Add(new QueryFieldValue {Key = z, Value = z});
                            });
                        });
                        return t.Result;
                    });
                    tasks.Add(dd);
                }
                result.AvailableQueryFields.Add(queryField);
            });
          
            Task.WaitAll(tasks.ToArray());
            return result;

        }

        public List<QueryFieldValue> GetFilteredQueryFields(string parentColumn, string dependentColumn, string parentValue)
        {
            var result = new List<QueryFieldValue>();

            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var collectionItems = db.GetCollection<BsonDocument>("incumbent");
            var tasks = new List<Task>();
            var filter = Builders<BsonDocument>.Filter.Eq(parentColumn, parentValue);
            FieldDefinition<BsonDocument, string> field = dependentColumn;
            var t = collectionItems.DistinctAsync<string>(field, filter);
            var z = Task.Factory.StartNew(() =>
            {
                var m = t.Result.ForEachAsync((s) =>
                {
                    result.Add(new QueryFieldValue { Key = s, Value = s });
                });
              
            });
            

            tasks.Add(z);
            Task.WaitAll(tasks.ToArray());
            return result;
        }
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

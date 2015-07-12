using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akron.Data;
using Akron.Data.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Akron.Tests.Data
{
    [TestClass]
    public class DataServiceTests
    {
        DataService GetTarget()
        {
            return new DataService();
        }
        [TestMethod]
        public void GetTotalCountTest()
        {
            var target = GetTarget();
            var actual = target.GetTotalCount();
            Assert.IsTrue(actual > 100);
        }

        [TestMethod]
        public void AverageTests()
        {
            var target = GetTarget();
            var actual = target.Average();
            Assert.IsTrue(actual > 100);
        }

        [TestMethod]
        public void GetDataTests()
        {
            var target = GetTarget();
            var queryDoc = new QueryDocument();
            var queryGroup = new GroupDefinition();
            queryGroup.Slicers.Add("Year");
            queryGroup.Slicers.Add("org_type");
            queryGroup.Facts.Add(new FactDefinition {Name = "Base_Pay", Operation = AggregateOperations.Average});
            queryDoc.CollectionName = "incumbent";
            queryDoc.Group = queryGroup;

            var actual = target.GetData(queryDoc);
            Assert.IsTrue(actual.Count == 90);
        }

        [TestMethod]
        public void PutMetaDataTest()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            BsonClassMap.RegisterClassMap<DataCollectionMetadata>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.CollectionName);

            });

            //db.CreateCollectionAsync("collectionMetadata").Wait();
            //t.Start();
            //t.Wait();

            var items = db.GetCollection<DataCollectionMetadata>("collectionMetadata");




            var s = new DataCollectionMetadata();
            s.CollectionName = "incumbent";
            s.DataSourceName = "hra";
            s.DisplayName = "Incumbent";

            var yearColumn = new DataColumnMetadata();
            yearColumn.ColumnIndex = 22;
            yearColumn.ColumnName = "Year";
            yearColumn.DataType = ColumnDataTypes.Int32;
            yearColumn.DisplayName = "Year";

            var orgType = new DataColumnMetadata();
            orgType.ColumnIndex = 7;
            orgType.ColumnName = "org_type";
            orgType.DataType = ColumnDataTypes.String;
            orgType.DisplayName = "Organization Type";

            var basePay = new DataColumnMetadata();
            basePay.ColumnIndex = 23;
            basePay.ColumnName = "Base_Pay";
            basePay.DataType = ColumnDataTypes.Double;
            basePay.DisplayName = "Base Pay";

            var jobFamily = new FilterDataColumnMetadata();
            jobFamily.ColumnIndex = 12;
            jobFamily.ColumnName = "Job_Family";
            jobFamily.DisplayName = "Job Family";
            basePay.DataType = ColumnDataTypes.String;

            var jobTrack = new FilterDataColumnMetadata();
            jobTrack.ColumnIndex = 14;
            jobTrack.ColumnName = "Job_Track";
            jobTrack.DisplayName = "Job Track";
            jobTrack.FilterDependencyColumns.Add(jobFamily);
            basePay.DataType = ColumnDataTypes.String;


            var jobLevel = new FilterDataColumnMetadata();
            jobLevel.ColumnIndex = 16;
            jobLevel.ColumnName = "Job_Level";
            jobLevel.DisplayName = "Job Level";
            basePay.DataType = ColumnDataTypes.String;

            s.Columns.Add(yearColumn);
            s.Columns.Add(orgType);
            s.Columns.Add(basePay);
            s.Columns.Add(jobFamily);
            s.Columns.Add(jobTrack);
            s.Columns.Add(jobLevel);

            s.Dimensions.Add(yearColumn);
            s.Dimensions.Add(jobFamily);
            s.Dimensions.Add(jobTrack);
            s.Dimensions.Add(jobLevel);


            s.Measures.Add(basePay);

            //do filters
            s.Filters.Add(jobFamily);
            s.Filters.Add(jobTrack);
            s.Filters.Add(jobLevel);
           
            items.InsertOneAsync(s).Wait();

        }

        [TestMethod]
        public void BuildQueryOptions()
        {
            var queryBuilder = new QueryBuilder();

            BsonClassMap.RegisterClassMap<DataCollectionMetadata>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.CollectionName);

            });
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<DataCollectionMetadata>("collectionMetadata");
            var collectionItems = db.GetCollection<BsonDocument>("incumbent");

            


            FilterDefinition<DataCollectionMetadata> filter = new BsonDocument("_id", "incumbent");
            var md = items.Find<DataCollectionMetadata>(filter);
            //var ct = md.CountAsync().Result;


            var metaData = md.SingleAsync().Result;
            var fValues = new List<String>();
            var tasks = new List<Task<IAsyncCursor<string>>>(); 
            metaData.Filters.ForEach(f =>
            {
                var queryField = new QueryField();

                queryField.Column = f;

                FieldDefinition<BsonDocument,string> field = f.ColumnName;
                var dd = Task<IAsyncCursor<string>>.Factory.StartNew(() =>
                {
                    var t = collectionItems.DistinctAsync<string>(field, new BsonDocument());
                    t.GetAwaiter().OnCompleted(() =>
                    {
                        t.Result.ForEachAsync((z) =>
                        {
                            queryField.AvailableValues.Add(new QueryFieldValue { Key = z, Value = z });
                        });
                    });
                    return t.Result;
                });
                //var dd = collectionItems.DistinctAsync<string>(field, new BsonDocument());
                //dd.GetAwaiter().OnCompleted(() =>
                //{
                //    //dd.Result.ForEachAsync<string>(x => (queryField.AvailableValues.Add(x)));
                //    dd.Result.ForEachAsync((z) =>
                //    {
                //        queryField.AvailableValues.Add(z);
                //    });
                //});
                tasks.Add(dd);
                queryBuilder.AvailableQueryFields.Add(queryField);
            });

            Task.WaitAll(tasks.ToArray());
           Assert.IsFalse(queryBuilder.AvailableQueryFields.Any(x=>x.AvailableValues.Count ==0));

        }

        [TestMethod]
        public void BuildQuery()
        {
            BsonClassMap.RegisterClassMap<DataCollectionMetadata>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c => c.CollectionName);

            });
            var ds = GetTarget();
            var result = ds.GetQueryBuilder("incumbent");
            //var result = new QueryBuilder();
            //var client = new MongoClient("mongodb://localhost:27017");
            //var db = client.GetDatabase("hra");
            //var items = db.GetCollection<DataCollectionMetadata>("collectionMetadata");
            //var collectionItems = db.GetCollection<BsonDocument>("incumbent");

            //FilterDefinition<DataCollectionMetadata> filter = new BsonDocument("_id", "incumbent");
            //var md = items.Find<DataCollectionMetadata>(filter);
            //var metadata = md.SingleAsync().Result;
            //var tasks = new List<Task<IAsyncCursor<string>>>();
            //metadata.Filters.ForEach(f =>
            //{
            //    var queryField = new QueryField();

            //    queryField.Column = f;

            //    FieldDefinition<BsonDocument, string> field = f.ColumnName;
            //    var dd = collectionItems.DistinctAsync<string>(field, new BsonDocument());
            //    dd.GetAwaiter().OnCompleted(() => dd.Result.ForEachAsync((z) => queryField.AvailableValues.Add(z)));
            //    tasks.Add(dd);
            //    result.AvailableQueryFields.Add(queryField);
            //});
            var x = "Y";
            //Task.WaitAll(tasks.ToArray());
            //Assert.IsFalse(result.AvailableQueryFields.Any(x => x.AvailableValues.Count == 0));
        }
    }
}

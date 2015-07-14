using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Akron.Data;
using Akron.Data.DataStructures;
using Akron.Data.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Akron.Tests.Data
{
    [TestClass]
    public class DataServiceTests
    {
        private DataService GetTarget()
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
            //var target = GetTarget();
            //var queryDoc = new QueryDocument();
            //var queryGroup = new GroupDefinition();
            //queryGroup.Slicers.Add("Year");
            //queryGroup.Slicers.Add("org_type");
            //queryGroup.Measures.Add(new FactDefinition {Name = "Base_Pay", Operation = AggregateOperations.Average});
            //queryDoc.CollectionName = "incumbent";
            //queryDoc.Group = queryGroup;

            //var actual = target.GetData(queryDoc);
            //Assert.IsTrue(actual.Count == 90);
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

        private QueryBuilder GetQueryBuilder()
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
            Assert.IsFalse(queryBuilder.AvailableQueryFields.Any(x => x.AvailableValues.Count == 0));

            var gd = new GroupDefinition();
     
            gd.Slicers.Add(new QueryField
            {
                Column = new DataColumnMetadata { ColumnName = "Year", DataType = ColumnDataTypes.Int32 }
            });

            var fd = new MeasureDefinition();
            fd.QueryField = new QueryField
            {
                Column = new DataColumnMetadata { ColumnName = "Base_Pay", DataType = ColumnDataTypes.Double }
            };
            gd.Measures.Add(fd);
            return queryBuilder;
        }

        [TestMethod]
        public void BuildQueryOptions()
        {
            //self asserting
            GetQueryBuilder();

        }


        [TestMethod]
        public void CascadeFilterTest()
        {
            List<QueryFieldValue> result = null;
            var target = GetTarget();
            result = target.GetFilteredQueryFields("Job_Family", "Job_Track", "Building & Facilities Maintenance");




            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void BuildQuery()
        {
            var source = GetQueryBuilder();
            var jobFamily = source.AvailableQueryFields.Single(x => x.Column.ColumnName == "Job_Family");
            var jobTrack = source.AvailableQueryFields.Single(x => x.Column.ColumnName == "Job_Track");

            var matchBuilder = new FilterDefinitionBuilder<BsonDocument>();
            jobFamily.SelectedValue = new QueryFieldValue {Key = "Executive", Value = "Executive"};
            var jobFamilyFilter = matchBuilder.All(jobFamily.Column.ColumnName,
                new List<String> {jobFamily.SelectedValue.Value});
            //take care of grouping key
            var groupDoc = new BsonDocument();
            var slicers = new List<BsonElement>();
            slicers.Add(new BsonElement("s1", new BsonString("$Year")));
            slicers.Add(new BsonElement("s2", new BsonString("$orgType")));
            var idDoc = new BsonDocument(slicers);
            var idElm = new BsonElement("_id", idDoc);

            var idDocument = new BsonDocument();
            idDocument.Add(idElm);

            //take care of grouping val
            var factDoc = new BsonDocument();
            var factEl = new BsonElement("$avg", new BsonString("$Base_Pay"));
            factDoc.Add(factEl);

            var factElementDoc = new BsonDocument();
            factElementDoc.Add(new BsonElement("f1", factDoc));
            var factDocResult = new BsonDocument();
            factDocResult.AddRange(factElementDoc);

            groupDoc.AddRange(idDocument);
            groupDoc.AddRange(factDocResult);

            var groupElement = new BsonElement("$group", groupDoc);

            var matchDoc = new BsonDocument();
            var filter1Element = new BsonElement("Job_Family", new BsonString("Executive"));
            var filter1Doc = new BsonDocument();
            filter1Doc.Add(filter1Element);
            var filterFinalElement = new BsonElement("$match", filter1Doc);

            var finalDoc = new BsonDocument();
            finalDoc.Add(filterFinalElement);
            finalDoc.Add(groupElement);

            var dd = finalDoc.ToString();
            Console.Out.Write(dd);
            var xx = "Y";




            // jobTrack.
        }

        [TestMethod]
        public void BuildQuerySimple()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
            var items = db.GetCollection<BsonDocument>("incumbent");

            var fd = new FilterDefinitionBuilder<BsonDocument>();
            var filter1 = fd.All("Job_Family", new List<string> {"Health Care"});




            IAggregateFluent<BsonDocument> dd = items.Aggregate();
            IAggregateFluent<BsonDocument> ts = dd.Match(filter1);
            var ll = ts.Group(x => new {Year = x["Year"], Org = x["org_type"]},
                g => new {Key=g.Key, Average=g.Average(x => (double) x["Base_Pay"])});

            ll.Project(x => new BsonDocument {new BsonElement("Key",x.Key.Year), new BsonElement("Average",x.Average)});
            //ts.Group()
            //IAggregateFluent<BsonDocument> gd = ts.Group(x => new {s1 = x["Year"], s2 = x["org_type"]},
            //        g => new {Key = g.Key, Value = g.Average(x => (double) x["Base_Pay"])}).Project(x => new {Key = new List<string>{x.Key.s1.ToString()}, Value = x.Value});
    

                //dd.Match(filter1)
                //.Group(x => new {s1 = x["Year"], s2 = x["org_type"]},
                //    g => new {Key = g.Key, Value = g.Average(x => (double) x["Base_Pay"])})
                //.Project(x => new {Key = new List<string>{x.Key.s1.ToString()}, Value = x.Value});
            
            //var dc = new List<IPipelineStageDefinition>();
            //dc.AddRange(dd.Stages);
            //var pl = dc.ToArray();
            //var zito = pl.ToJson();
            //var docs = items.AggregateAsync<BsonDocument>(dc);
            var zz = ll.ToListAsync().Result;
            Assert.IsTrue(zz.Count > 0);
          
            var pp = "cc";
        }

        GroupDefinition GetGroupDefinition()
        {
            var result = new GroupDefinition();
            //possible slicers
            var jobFamily = new DataColumnMetadata {ColumnName = "Job_Family"};
            var jobFamilyValue = new QueryFieldValue {Key = "Health Care", Value="Health Care"};
            var jobTrack = new DataColumnMetadata {ColumnName = "Job_Track" };
            //slicer remains constant when selected
            var yearColumn = new DataColumnMetadata {ColumnName = "Year"};
            //measure 

            var basePayColumn = new DataColumnMetadata {ColumnName = "Base_Pay"};
            var basePayMeasure = new MeasureDefinition
            {
                Operation = AggregateOperations.Average,
                QueryField = new QueryField {Column = basePayColumn}
            };
            //TODO no Selected Value thats a filter
            result.Slicers.Add(new QueryField
            {
                Column = jobFamily
            });
            result.Slicers.Add(new QueryField {Column = yearColumn});
            result.Measures.Add(basePayMeasure);

            return result;
          
        }

        MatchDefinition GetMatchDefinition()
        {
            var mf = new QueryField
            {
                Column = new DataColumnMetadata { ColumnName = "Job_Family", DataType = ColumnDataTypes.String },
                SelectedValue = new QueryFieldValue { Key = "Health Care", Value = "Health Care" }
            };

            var matchDefinition = new MatchDefinition();
            matchDefinition.Filters.Add(mf);


            return matchDefinition;
        }

        [TestMethod]
        public void QueryBuilderMapTest()
        {
            
            var matchDefinition = GetMatchDefinition();
            var groupDefinition = GetGroupDefinition();
            var md = matchDefinition.ToMatchDocument().ToString();
            var gd = groupDefinition.ToGroupDocument().ToString();
            Console.Out.Write(md);
            Console.Out.Write(gd);
            var queryDoc = new QueryDocument { Group = groupDefinition, Match = matchDefinition, CollectionName = "incumbent", DataSourceLocation = "mongodb://localhost:27017", DataSource="hra" };
            var target = GetTarget();
            var actual = target.GetData(queryDoc);
            Assert.IsTrue(actual.Count > 0);



        }

        [TestMethod]
        public void MatchDefTests()
        {


            var xs = GetMatchDefinition().ToBsonDocument().ToString();
            Console.Out.Write(xs);
        }

    }
 

    //public class GResult<T>
    //{
    //    public object Key { get; set; }
    //    public T Value { get; set; }
    //}
}

   


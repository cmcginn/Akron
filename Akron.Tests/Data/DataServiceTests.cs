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
            //queryGroup.Dimensions.Add("Year");
            //queryGroup.Dimensions.Add("org_type");
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
            //Dimensions are slicers
            var yearColumnDimension = new DimensionColumnMetadata();
            yearColumnDimension.ColumnIndex = 22;
            yearColumnDimension.ColumnName = "Year";
            yearColumnDimension.DataType = ColumnDataTypes.Int32;
            yearColumnDimension.DisplayName = "Year";
            yearColumnDimension.IsDefault = true;

            var jobFamilyDimension = new DimensionColumnMetadata();
            jobFamilyDimension.ColumnIndex = 12;
            jobFamilyDimension.ColumnName = "Job_Family";
            jobFamilyDimension.DisplayName = "Job Family";
            jobFamilyDimension.DataType = ColumnDataTypes.String;

            var jobTrackDimension = new DimensionColumnMetadata();
            jobTrackDimension.ColumnIndex = 14;
            jobTrackDimension.ColumnName = "Job_Track";
            jobTrackDimension.DisplayName = "Job Track";
            jobTrackDimension.DataType = ColumnDataTypes.String;


            var jobLevelDimension = new DimensionColumnMetadata();
            jobLevelDimension.ColumnIndex = 16;
            jobLevelDimension.ColumnName = "Job_Level";
            jobLevelDimension.DisplayName = "Job Level";
            jobLevelDimension.DataType = ColumnDataTypes.String;

            var orgTypeDimension = new DimensionColumnMetadata();
            orgTypeDimension.ColumnIndex = 7;
            orgTypeDimension.ColumnName = "org_type";
            orgTypeDimension.DisplayName = "Organization Type";
            orgTypeDimension.DataType = ColumnDataTypes.String;
            
            var orgType = new DimensionColumnMetadata();
            orgType.ColumnIndex = 7;
            orgType.ColumnName = "org_type";
            orgType.DataType = ColumnDataTypes.String;
            orgType.DisplayName = "Organization Type";

            var basePay = new MeasureDataColumnMetadata();
            basePay.ColumnIndex = 23;
            basePay.ColumnName = "Base_Pay";
            basePay.DataType = ColumnDataTypes.Double;
            basePay.DisplayName = "Base Pay";
            basePay.IsDefault = true;

            var yearColumn = new DataColumnMetadata();
            yearColumn.ColumnIndex = 22;
            yearColumn.ColumnName = "Year";
            yearColumn.DataType = ColumnDataTypes.Int32;
            yearColumn.DisplayName = "Year";
    

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

            s.Dimensions.Add(yearColumnDimension);
            s.Dimensions.Add(jobFamilyDimension);
            s.Dimensions.Add(jobTrackDimension);
            s.Dimensions.Add(jobLevelDimension);
            s.Dimensions.Add(orgTypeDimension);


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
                var filterDefinition = new FilterDefinition();

                filterDefinition.Column = f;

                FieldDefinition<BsonDocument, string> field = f.ColumnName;
                var dd = Task<IAsyncCursor<string>>.Factory.StartNew(() =>
                {
                    var t = collectionItems.DistinctAsync<string>(field, new BsonDocument());
                    t.GetAwaiter().OnCompleted(() =>
                    {
                        t.Result.ForEachAsync((z) =>
                        {
                            filterDefinition.AvailableFilterValues.Add(new FilterValue {Key = z, Value = z});
                        });
                    });
                    return t.Result;
                });

                tasks.Add(dd);
                queryBuilder.AvailableFilters.Add(filterDefinition);
            });

            Task.WaitAll(tasks.ToArray());
            Assert.IsFalse(queryBuilder.SelectedFilters.Any());

            var gd = new GroupDefinition();
     
            gd.Dimensions.Add(new DimensionDefinition
            {
                Column = new DataColumnMetadata { ColumnName = "Year", DataType = ColumnDataTypes.Int32 },
                IsDefault=true
            });

            var fd = new MeasureDefinition();
            fd.Column = new DataColumnMetadata {ColumnName = "Base_Pay", DataType = ColumnDataTypes.Double};
            fd.IsDefault = true;
                
          
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
            List<FilterValue> result = null;
            var target = GetTarget();
            result = target.GetFilteredQueryFields("Job_Family", "Job_Track", new List<string>{"Building & Facilities Maintenance"});




            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void BuildQuery()
        {
            var source = GetQueryBuilder();
            var jobFamily = source.AvailableFilters.Single(x => x.Column.ColumnName == "Job_Family");
            var jobTrack = source.AvailableFilters.Single(x => x.Column.ColumnName == "Job_Track");

            var matchBuilder = new FilterDefinitionBuilder<BsonDocument>();
            jobFamily.AvailableFilterValues.Add(new FilterValue {Key = "Executive", Value = "Executive", Active=true});
 
            var jobFamilyFilter = matchBuilder.All(jobFamily.Column.ColumnName,
                jobFamily.AvailableFilterValues.Where(x=>x.Active).Select(x=>x.Value).ToList());
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
            var jobFamilyValue = new FilterValue {Key = "Health Care", Value="Health Care"};
            var jobTrack = new DataColumnMetadata {ColumnName = "Job_Track" };
            //slicer remains constant when selected
            var yearColumn = new DataColumnMetadata {ColumnName = "Year"};
            
            //measure 

            var basePayColumn = new DataColumnMetadata {ColumnName = "Base_Pay"};
            var basePayMeasure = new MeasureDefinition
            {
                Operation = AggregateOperations.Average,
                Column = basePayColumn,
                IsDefault=true
            };
            //TODO no Selected Value thats a filter
            result.Dimensions.Add(new DimensionDefinition { Column = yearColumn, IsDefault=true });
            result.Dimensions.Add(new DimensionDefinition
            {
                Column = jobFamily
            });
            
            result.Measures.Add(basePayMeasure);

            return result;
          
        }

        MatchDefinition GetMatchDefinition()
        {
            var mf = new FilterDefinition
            {
                Column = new DataColumnMetadata { ColumnName = "Job_Family", DataType = ColumnDataTypes.String }
               
            };
            mf.AvailableFilterValues.Add(new FilterValue {Key = "Executive", Value = "Executive", Active=true});
            var matchDefinition = new MatchDefinition();
            matchDefinition.Filters.Add(mf);


            return matchDefinition;
        }

        [TestMethod]
        public void QueryBuilderMapTest()
        {

            var start = System.DateTime.UtcNow;
            var matchDefinition = GetMatchDefinition();
           matchDefinition.Filters.Clear();
            var groupDefinition = GetGroupDefinition();
           // matchDefinition.Filters.Clear();
            //matchDefinition.Filters.Add(new FilterDefinition
            //{
            //    Column = new DataColumnMetadata { ColumnName = "Legal" }
            //});
            //matchDefinition.Filters.First().AvailableFilterValues.Add(new FilterValue {Key = "Legal", Value = "Legal", Active=true});
            var md = matchDefinition.ToMatchDocument();
            var gd = groupDefinition.ToGroupDocument();
            var pd = groupDefinition.ToProjectionDocument();
            var sg = QueryBuilderExtensions.ToGroupSeriesDocument();
            var sp = QueryBuilderExtensions.ToProjectSeriesDocument();
            //Console.Out.Write(md.ToString());
            //Console.Out.Write(gd.ToString());
            //Console.Out.Write(pd.ToString());

            var queryDoc = new QueryDocument
            {
                CollectionName = "incumbent",
                DataSourceLocation = "mongodb://localhost:27017",
                DataSource = "hra"
            };
            //queryDoc.Pipeline.Add(md);
            queryDoc.Pipeline.Add(gd);
            //queryDoc.Pipeline.Add(pd);
            //queryDoc.Pipeline.Add(sg);
            //queryDoc.Pipeline.Add(sp);
            //Console.Out.WriteLine(gd);

            //queryDoc.Pipeline.Add(gd);
            //Console.Out.WriteLine(gd.ToString());
            //Console.Out.WriteLine(pd.ToString());
            //Console.Out.WriteLine(sg.ToString());
            //Console.Out.WriteLine(sp.ToString());
           var doc =
   BsonDocument.Parse(
       "{ '$group' : { '_id' : { 's0' : '$Year','s1' : '$Job_Family' }, 'f0' : { '$avg' : '$Base_Pay' } } }");
            //queryDoc.Pipeline.Clear();
            //queryDoc.Pipeline.Add(md);
            //queryDoc.Pipeline.Add(gd);
           var target = GetTarget();
           List<SeriesGrid> result = new List<SeriesGrid>();
            var actual = target.GetData(queryDoc).ToList();//.ForEach(x =>
           //{
           //    var item = new SeriesGrid();
           //    item.Key = x["key"];
           //    var values = x["f0"] as BsonArray;
           //    item.Values = new List<List<object>>();
           //    for (var i = 0; i < values.Count; i++)
           //    {
           //        var valueList = new List<object>();
           //        valueList.Add(x["f0"][i]["s1"]);
           //        valueList.Add(x["f0"][i]["f0"]);
           //        item.Values.Add(valueList);
           //    }
           //    result.Add(item);
           //});
        

           Assert.IsTrue(actual.Count > 0);
           var finish = (System.DateTime.UtcNow - start).TotalMilliseconds;
            Console.Out.WriteLine(finish);


        }

        [TestMethod]
        public void BuildProjectTest()
        {
            var groupDefinition = GetGroupDefinition();
            var keyItems = new List<BsonElement>();
            var valueItems = new List<BsonElement>();

            var ignoreId = new BsonElement("_id", new BsonInt32(0));
          
          
            for (var i = 0; i < groupDefinition.Dimensions.Count; i++)
            {
                var el = new BsonElement(String.Format("s{0}", i), new BsonString(String.Format("$_id.s{0}", i)));
                keyItems.Add(el);
            }
            for (var i = 0; i < groupDefinition.Measures.Count; i++)
            {
                var el = new BsonElement(String.Format("f{0}", i), new BsonString(String.Format("$f{0}", i)));
                valueItems.Add(el);
            }

            var keyValuesDoc = new BsonDocument();
            keyValuesDoc.AddRange(keyItems);
            var keyValuesElement = new BsonElement("key", keyValuesDoc);

            var valueValuesDoc = new BsonDocument();
            valueValuesDoc.AddRange(valueItems);

            var valueValuesElement = new BsonElement("value", valueValuesDoc);

            var ignoreIdDoc = new BsonDocument();
            ignoreIdDoc.Add();

            var projectDoc = new BsonDocument();
            projectDoc.Add(new BsonElement("_id", new BsonInt32(0)));
            projectDoc.Add(keyValuesElement);
            projectDoc.Add(valueValuesElement);
            var projectElement = new BsonElement("$project", projectDoc);
            var finalDoc = new BsonDocument();
            finalDoc.Add(projectElement);
            //var projectValuesDoc  = new BsonDocument();

            //projectValuesDoc.AddRange(keyItems);

            //var projectItemsElement = new BsonElement("$project", projectValuesDoc);
            //var finalDoc = new BsonDocument();
            //finalDoc.Add(projectItemsElement);
            Console.Out.Write(finalDoc.ToString());

        }
        [TestMethod]
        public void MatchDefTests()
        {


            var xs = GetMatchDefinition().ToBsonDocument().ToString();
            Console.Out.Write(xs);
        }

        [TestMethod]
        public void BuildDefaultQuery()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");
 
            var items = db.GetCollection<BsonDocument>("incumbent");

            var qb = new QueryBuilder();
           // qb.SelectedSlicers
        }

        [TestMethod]
        public void BenchmarkTest()
        {
            var start = System.DateTime.UtcNow;
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");

            var items = db.GetCollection<BsonDocument>("incumbent");

            var gg = items.Aggregate()
                .Group(x => new {year = x["Year"], orgType = x["org_type"]},
                    g => new {Key=g.Key, V=g.Average(x => (double) x["Base_Pay"])});

            var m = gg.ToListAsync().Result;
            Assert.IsTrue(m.Count > 0);
            var finish = (System.DateTime.UtcNow - start).TotalMilliseconds;
            Console.Out.WriteLine(finish);
        }

        [TestMethod]
        public void BenchmarkTest2()
        {
            var start = System.DateTime.UtcNow;
            var doc =
                BsonDocument.Parse(
                    "{ '$group' : { '_id' : { 's0' : '$Year','s1' : '$org_type' }, 'f0' : { '$avg' : '$Base_Pay' } } }");
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("hra");

            var items = db.GetCollection<BsonDocument>("incumbent");

            var gg = items.AggregateAsync<BsonDocument>(new[] {doc});
            var m = gg.Result.ToListAsync().Result;
            Assert.IsTrue(m.Count > 0);
            var finish = (System.DateTime.UtcNow - start).TotalMilliseconds;
            Console.Out.WriteLine(finish);
        }

        [TestMethod]
        public void SeriesTest()
        {
           // var queryDoc = new QueryDocument();
           // var queryGroup = new GroupDefinition();

           // var qb = new QueryBuilder();

           // qb.SelectedSlicers.Add(new DimensionDefinition
           // {
           //     Column = new DataColumnMetadata { ColumnName = "Year" },
           //     IsDefault = true
           // });
           // qb.SelectedSlicers.Add(new DimensionDefinition
           // {
           //     Column = new DataColumnMetadata { ColumnName = "org_type" }

           // });
           // qb.SelectedMeasures = new List<MeasureDefinition>
           //{
           //    new MeasureDefinition
           //    {
           //        Column = new DataColumnMetadata {ColumnName = "Base_Pay"},
           //        IsDefault = true,
           //        Operation = AggregateOperations.Average
           //    }
           //};
           // var target = GetTarget();
           // var qd = qb.ToQueryDocument();
           // qd.CollectionName = "incumbent";
           // qd.DataSource = "hra";
           // qd.DataSourceLocation = "mongodb://localhost:27017";
           // var result = new List<BsonDocument>();
           // target.GetData(qd).GroupBy(x => x["_id"]["s0"]).ToList().ForEach(x =>
           // {
           //     var item = new BsonDocument();
           //     var arr = new BsonArray();
           //     var key = new BsonElement("key",x.Key["s0"])
                

           // });
           // var rrrr = "";
        }
    }


    //public class GResult<T>
    //{
    //    public object Key { get; set; }
    //    public T Value { get; set; }
    //}
}

public class SeriesGrid
{
    public object Key { get; set; }
    public List<List<object>> Values { get; set; }
}
   


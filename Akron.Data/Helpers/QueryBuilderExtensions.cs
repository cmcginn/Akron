using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akron.Data.DataStructures;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Akron.Data.Helpers
{
    public static class QueryBuilderExtensions
    {
        public static QueryDocument ToSeriesQueryDocument(this QueryBuilder source)
        {
            var result = source.ToQueryDocument();
            result.Pipeline.Add(ToGroupSeriesDocument());
            result.Pipeline.Add(ToProjectSeriesDocument());
            return result;
        }
        public static QueryDocument ToQueryDocument(this QueryBuilder source)
        {
            var result = new QueryDocument();
            var match = new MatchDefinition();
           
            match.Filters = source.SelectedFilters;
            var group = new GroupDefinition();
            group.Measures = source.SelectedMeasures;
            
            group.Dimensions = source.SelectedSlicers;
            var project = group.ToProjectionDocument();

            result.Pipeline.Add(match.ToMatchDocument());
            result.Pipeline.Add(group.ToGroupDocument());
            result.Pipeline.Add(project);
            return result;
        }

        public static BsonDocument ToProjectSeriesDocument()
        {
            var result = new BsonDocument();
            var projectElements = new List<BsonElement>();
            //supressId
            projectElements.Add(new BsonElement("_id", new BsonInt32(0)));
            projectElements.Add(new BsonElement("key", new BsonString("$_id")));
            projectElements.Add(new BsonElement("f0", new BsonString("$f0")));
            result.Add(new BsonElement("$project", new BsonDocument(projectElements)));
            return result;
        }
        public static BsonDocument ToGroupSeriesDocument()
        {
            var result = new BsonDocument();
            var finalElements = new List<BsonElement>();
            //first slicer will always be first element in ID representing x axis
            finalElements.Add(new BsonElement("_id", new BsonString("$key.s0")));
            //push first
            var pushElements = new List<BsonElement>();
            //second slicer will always be second element in Key
            pushElements.Add(new BsonElement("s1", new BsonString("$key.s1")));
            //value will always be first element in value
            pushElements.Add(new BsonElement("f0", new BsonString("$value.f0")));
            var pushElement = new BsonElement("$push", new BsonDocument(pushElements));
            var pushField = new BsonElement("f0", new BsonDocument(pushElement));
            finalElements.Add(pushField);
            result.Add(new BsonElement("$group", new BsonDocument(finalElements)));
            return result;
        }
        public static BsonDocument ToMatchDocument(this MatchDefinition source)
        {
            var result = new BsonDocument();
            var matchFilterElements = new List<BsonElement>();
            source.Filters.ForEach(f =>
            {
                var colDoc = new BsonDocument();
                var itemElm = new BsonElement("$eq", new BsonString(f.FilterValue.Value));
                colDoc.Add(itemElm);
                var colElm = new BsonElement(f.Column.ColumnName, colDoc);
                matchFilterElements.Add(colElm);
            });
            var elementsDoc = new BsonDocument();
            elementsDoc.AddRange(matchFilterElements);

            var matchElement = new BsonElement("$match", elementsDoc);
            result.Add(matchElement);
            return result;
        }

        public static BsonDocument ToGroupDocument(this GroupDefinition source)
        {
            var result = new BsonDocument();
            result.AddRange(source.ToGroup());
            return result;
        }

        public static BsonDocument ToProjectionDocument(this GroupDefinition source)
        {
            var keyItems = new List<BsonElement>();
            var valueItems = new List<BsonElement>();

            var ignoreId = new BsonElement("_id", new BsonInt32(0));


            for (var i = 0; i < source.Dimensions.Count; i++)
            {
                var el = new BsonElement(String.Format("s{0}", i), new BsonString(String.Format("$_id.s{0}", i)));
                keyItems.Add(el);
            }
            for (var i = 0; i < source.Measures.Count; i++)
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
            var result = new BsonDocument {projectElement};
            return result;
        }
        static IEnumerable<BsonElement> ToGroup(this GroupDefinition source)
        {
            var result = new BsonDocument();
            var groupDoc = new BsonDocument();
            //
            groupDoc.AddRange(source.ToId());
            groupDoc.AddRange(source.ToFact());
            var groupElement = new BsonElement("$group", groupDoc);
            result.Add(groupElement);
            return result;
        }
        static IEnumerable<BsonElement> ToId(this GroupDefinition source)
        {
            var result = new BsonDocument();


            var slicers = new List<BsonElement>();
          
            for (var i = 0; i < source.Dimensions.Count; i++)
            {
                slicers.Add(new BsonElement(String.Format("s{0}", i), new BsonString(String.Format("${0}", source.Dimensions.ElementAt(i).Column.ColumnName))));
            }
            var idDoc = new BsonDocument(slicers);
            var idElm = new BsonElement("_id", idDoc);
            result.Add(idElm);
            return result;

        }
        static IEnumerable<BsonElement> ToFact(this GroupDefinition source)
        {
   
            var result = new BsonDocument();
            for (var i = 0; i < source.Measures.Count; i++)
            {
                var fact = source.Measures[i];
                var op = "";
                switch (fact.Operation)
                {
                    case AggregateOperations.Average:
                        op = "$avg";
                        break;
                    case AggregateOperations.Sum:
                        op = "$sum";
                        break;
                }
                var el = new BsonElement(op, new BsonString(String.Format("${0}", fact.Column.ColumnName)));
                var factDoc = new BsonDocument();
                factDoc.Add(el);
                var factElementDoc = new BsonDocument();
                factElementDoc.Add(new BsonElement(String.Format("f{0}", i), factDoc));
                result.AddRange(factElementDoc);

            }
            return result;
        }
    }
}

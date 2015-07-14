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

        public static QueryDocument ToQueryDocument(this QueryBuilder source)
        {
            var result = new QueryDocument();
            result.Group = new GroupDefinition();
            result.Group.Measures = source.AvailableMeasures.Where(x => x.QueryField.SelectedValue != null).ToList();
            //TODO refactor this, these do not have selected values
            result.Group.Slicers = source.AvailableSlicers.Where(x => x.SelectedValue != null).ToList();

            result.Match = new MatchDefinition();
            //TODO rename to available filters
            result.Match.Filters = source.AvailableQueryFields.Where(x => x.SelectedValue != null).ToList();
            return result;
        }

        public static BsonDocument ToMatchDocument(this MatchDefinition source)
        {
            var result = new BsonDocument();
            var matchFilterElements = new List<BsonElement>();
            source.Filters.ForEach(f =>
            {
                var colDoc = new BsonDocument();
                var itemElm = new BsonElement("$eq", new BsonString(f.SelectedValue.Value));
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
          
            for (var i = 0; i < source.Slicers.Count; i++)
            {
                slicers.Add(new BsonElement(String.Format("s{0}", i), new BsonString(String.Format("${0}", source.Slicers.ElementAt(i).Column.ColumnName))));
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
                var el = new BsonElement(op, new BsonString(String.Format("${0}", fact.QueryField.Column.ColumnName)));
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

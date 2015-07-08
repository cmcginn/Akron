using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akron.Data.DataStructures;
using MongoDB.Bson;

namespace Akron.Data.Helpers
{
    public static class QueryBuilder
    {

        public static BsonDocument ToGroup(this GroupDefinition source)
        {
            var result = new BsonDocument();

            result.AddRange(source.ToGroupDocument());

            return result;
        }
        static IEnumerable<BsonElement> ToGroupDocument(this GroupDefinition source)
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
                slicers.Add(new BsonElement(String.Format("s{0}", i), new BsonString(String.Format("${0}", source.Slicers.ElementAt(i)))));
            }
            var idDoc = new BsonDocument(slicers);
            var idElm = new BsonElement("_id", idDoc);
            result.Add(idElm);
            return result;

        }
        static IEnumerable<BsonElement> ToFact(this GroupDefinition source)
        {
            var result = new BsonDocument();
            for (var i = 0; i < source.Facts.Count; i++)
            {
                var fact = source.Facts[i];
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
                var el = new BsonElement(op, new BsonString(String.Format("${0}", fact.Name)));
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

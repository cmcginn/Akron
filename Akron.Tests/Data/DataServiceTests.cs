using System;
using Akron.Data;
using Akron.Data.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
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

    }
}

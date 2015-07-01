using System;
using Akron.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}

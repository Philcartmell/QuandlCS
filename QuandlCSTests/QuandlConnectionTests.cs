using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuandlCS.Connection;

namespace QuandlCSTests
{
    [TestClass]
    public class QuandlConnectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Request_null_throws()
        {
            var connection = new QuandlConnection();
            connection.Request(null);
        }
    }
}

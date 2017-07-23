using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SberDat.Models;
using System.Collections.Generic;

namespace SberDat.Tests
{
    [TestClass]
    public class ElementTests
    {
        [TestMethod]
        public void TestEmptyList()
        {
            Assert.AreEqual("<table border=\"1\"></table>", Element.generateTable(new List<Element>()));
        }
    }
}

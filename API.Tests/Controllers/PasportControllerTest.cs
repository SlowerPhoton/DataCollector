using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using API;
using API.Controllers;
using System.Collections.Generic;

namespace API.Tests.Controllers
{
    [TestClass]
    public class PasportControllerTest
    {
        [TestMethod]
        public void GetICOAndID()
        {
            PasportController pc = new PasportController();
            IEnumerable<object> ret =  pc.Get("456", "555");
            Assert.IsNull(ret);
        }

    }
}

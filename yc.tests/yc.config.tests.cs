using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yc.config;

namespace yc.tests
{
    [TestClass]
    public class ycConfigTests
    {
        [TestMethod]
        public void GetEndpointByIdTest()
        {
           var ret =  YcConfig.Instance.Configuration["Settings:compute"];
        }
    }
}

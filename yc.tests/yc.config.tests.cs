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
        public async Task GetEndpointByIdTest()
        {
           var ret =  await YcConfig.GetEndpointById("compute");
        }
    }
}

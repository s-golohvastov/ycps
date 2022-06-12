using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using yc.config;
using yc.resourcemanager;

namespace yc.tests
{
    [TestClass]
    public class ycResourceManagerTests
    {
        static private PowerShell _powershell;

        [ClassInitialize()]
        public static void InitPsGraphUnitTest(TestContext tc)
        {
            _powershell = PowerShell.Create();
            _powershell.AddCommand("Import-Module")
                .AddParameter("Assembly", System.Reflection.Assembly.GetAssembly(typeof(GetYcCloudCmdlet)));
            _powershell.Invoke();
            _powershell.Commands.Clear();
        }

        [ClassCleanup()]
        public static void CleanupPsGraphUnitTest()
        {
            _powershell?.Dispose();
        }


        [TestMethod]
        public async Task GetYcCloudByCloudIdString()
        {
            try
            {
                _powershell.AddCommand("Get-YcCloud");
                _powershell.AddParameters(new Dictionary<String, Object>
                    {
                        {"CloudId", YcConfig.Configuration["Secrets:CloudId"]}
                    });
                Collection<PSObject> result = _powershell.Invoke();

            }
            finally
            {
                _powershell.Commands.Clear();
            }
        }

        public async Task GetYcCloudByOrganizationObject()
        {
            try
            {
                _powershell.AddCommand("Connect-YcAccount");
                _powershell.AddParameters(new Dictionary<String, Object>
                    {
                        {"OAuthToken", YcConfig.Configuration["Secrets:OAuthToken"]}
                    });
                Collection<PSObject> result = _powershell.Invoke();

            }
            finally
            {
                _powershell.Commands.Clear();
            }
        }
    }
}

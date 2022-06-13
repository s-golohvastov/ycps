using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using yc.auth;
using yc.config;
using yc.connect;
using yc.orgmgr;

namespace yc.tests
{
    [TestClass]
    public class YcOrgmgrTests
    {
        static private PowerShell _powershell;

        [ClassInitialize()]
        public static void InitPsGraphUnitTest(TestContext tc)
        {
            _powershell = PowerShell.Create();
            
            _powershell.AddCommand("Import-Module")
                .AddParameter("Assembly", System.Reflection.Assembly.GetAssembly(typeof(ConnectYCAccountCmdlet)));
            _powershell.Invoke();
            _powershell.Commands.Clear();
            
            _powershell.AddCommand("Import-Module")
                .AddParameter("Assembly", System.Reflection.Assembly.GetAssembly(typeof(GetYcOrganizationCmdlet)));
            _powershell.Invoke();
            _powershell.Commands.Clear();
        }

        [ClassCleanup()]
        public static void CleanupPsGraphUnitTest()
        {
            _powershell?.Dispose();
        }
        [TestMethod]
        public void GetYcOrganizationTest()
        {
            try
            {
                _powershell.AddCommand("Connect-YcAccount");
                _powershell.AddParameters(new Dictionary<String, Object>
                    {
                        {"OAuthToken", YcConfig.Instance.Configuration["Secrets:OAuthToken"]}
                    });
                Collection<PSObject> result = _powershell.Invoke();
                _powershell.Commands.Clear();

                _powershell.AddCommand("Get-YcOrganization");
                result = _powershell.Invoke();

            }
            finally
            {
                _powershell.Commands.Clear();
            }
        }
    }
}

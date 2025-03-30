using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.BiDi.Modules.BrowsingContext;

namespace mantis_tests
{
    public class AuthTestBase : TestBase
    {
        [OneTimeSetUp]
        public void SetupLogin()
        {
            app.Navigator.OpenHomePage();
            app.Auth.Login(new AccountData("administrator", "root"));
        }
    }
}

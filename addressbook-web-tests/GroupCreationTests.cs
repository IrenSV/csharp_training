using System;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class GroupCreationTests : TestBase
    {

        [Test]
        public void GroupCreationTest()
        {
            OpenHomePage();
            Login(new AccountData("admin", "secret"));
            GoToGroupsPage();
            InitNewGroupCreation();
            GroupData group = new GroupData("gr1");
            group.Header = "gr1";
            group.Footer = "gr1";
            FillGroupForm(group);
            SubmitGroupCreation();
            ReturnToGroupsPage();
            ReturnToHomePage();
        }
    }
}

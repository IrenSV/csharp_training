using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class ContactModificationTests : AuthTestBase
    {
        [Test]
        public void ContactModificationTest()
        {
            app.Contacts.Check();
            ContactData newData = new ContactData("Name", "LastName");

            app.Contacts.Modify(1, newData);
        }

    }
}

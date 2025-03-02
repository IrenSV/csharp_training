﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class ContactCreationTests : AuthTestBase
    {
        public static IEnumerable<ContactData> RandomGroupDataProvider()
        {
            List<ContactData> contacts = new List<ContactData>();
            for (int i = 0; i < 5; i++)
            {
                contacts.Add(new ContactData(GenerateRandomString(30), GenerateRandomString(30)));
            }
            return contacts;
        }
        [Test, TestCaseSource("RandomGroupDataProvider")]
        public void ContactCreationTest(ContactData contact)
        {
            List<ContactData> oldContacts = app.Contacts.GetContactList();

            app.Contacts.Create(contact);

            Assert.AreEqual(oldContacts.Count + 1, app.Contacts.GetContactCount());

            List<ContactData> newContacts = app.Contacts.GetContactList();
            oldContacts.Add(contact);
            oldContacts.Sort();
            newContacts.Sort();
            Assert.AreEqual(oldContacts, newContacts);
        }

        [Test]
        public void BadNameContactCreationTest()
        {
            ContactData contact = new ContactData("a'a", "");

            List<ContactData> oldContacts = app.Contacts.GetContactList();

            app.Contacts.Create(contact);

            Assert.AreEqual(oldContacts.Count + 1, app.Contacts.GetContactCount());

            List<ContactData> newContacts = app.Contacts.GetContactList();
            oldContacts.Add(contact);
            oldContacts.Sort();
            newContacts.Sort();
            Assert.AreEqual(oldContacts, newContacts);
        }

    }
}

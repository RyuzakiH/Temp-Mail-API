using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TempMail.API;

namespace TempMail.Tests
{
    [TestClass]
    public class TempMailTests
    {
        private static TempMailClient client;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            client = TempMailClient.Create();
        }

        [TestMethod]
        public void TestCreate()
        {
            Assert.IsInstanceOfType(client, typeof(TempMailClient));
        }

        [TestMethod]
        public void TestAvailableDomains()
        {
            Assert.IsNotNull(client.AvailableDomains);

            Assert.IsTrue(client.AvailableDomains.Any());
        }

        [TestMethod]
        public void TestEmail()
        {
            Assert.IsNotNull(client.Email);
        }

        [TestMethod]
        public void TestInbox()
        {
            Assert.IsNotNull(client.Inbox);
        }

        [TestMethod]
        public void TestChangeEmail()
        {
            client.ChangeEmail("logintest", client.AvailableDomains[0]);

            Assert.AreEqual($"logintest{client.AvailableDomains[0]}", client.Email);
        }

        [TestMethod]
        public void TestDelete()
        {
            var old_email = client.Email;

            client.Delete();

            Assert.AreNotEqual(old_email, client.Email);
        }

        [ClassCleanup]
        public static void TestCleanup()
        {
            client.Dispose();
        }
    }
}

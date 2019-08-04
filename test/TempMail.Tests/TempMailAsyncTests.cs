using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using TempMail.API;

namespace TempMail.Tests
{
    [TestClass]
    public class TempMailAsyncTests
    {
        private static TempMailClient client;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            client = await TempMailClient.CreateAsync();
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
        public async Task TestChangeEmail()
        {
            await client.ChangeEmailAsync("logintest", client.AvailableDomains[0]);

            Assert.AreEqual($"logintest{client.AvailableDomains[0]}", client.Email);
        }

        [TestMethod]
        public async Task TestDelete()
        {
            var old_email = client.Email;

            await client.DeleteAsync();

            Assert.AreNotEqual(old_email, client.Email);
        }

        [ClassCleanup]
        public static void TestCleanup()
        {
            client.Dispose();
        }
    }
}

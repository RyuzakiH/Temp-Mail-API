using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TempMail.API;

namespace TempMail.Tests
{
    [TestClass]
    public class InboxTests
    {
        private static TempMailClient client;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            client = TempMailClient.Create();
        }

        [TestMethod]
        public void TestMails()
        {
            var client = TempMailClient.Create();

            Assert.IsNotNull(client.Inbox.Mails);
        }

        [TestMethod]
        public void TestRefresh()
        {
            var client = TempMailClient.Create();

            var mails = client.Inbox.Refresh();

            Assert.AreSame(mails, client.Inbox.Mails);
        }

        [TestMethod]
        public async Task TestRefreshAsync()
        {
            var client = await TempMailClient.CreateAsync();

            var mails = await client.Inbox.RefreshAsync();

            Assert.AreSame(mails, client.Inbox.Mails);
        }

        [ClassCleanup]
        public static void TestCleanup()
        {
            client.Dispose();
        }
    }
}

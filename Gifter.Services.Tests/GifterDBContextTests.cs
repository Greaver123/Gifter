using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Gifter.Services.Tests
{
    [TestClass]
    public class GifterDBContextTests: TestWithSQLiteBase
    {
        [TestMethod]
        public async Task DatabaseIsAvailableAndCanBeConnectedTo()
        {
            Assert.IsTrue(await DbContext.Database.CanConnectAsync());
        }
    }
}

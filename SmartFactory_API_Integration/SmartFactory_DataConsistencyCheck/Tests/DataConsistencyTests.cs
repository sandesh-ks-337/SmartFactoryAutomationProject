using NUnit.Framework;
using NUnit.Framework.Legacy;
using SmartFactory_DataConsistencyCheck.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace SmartFactory_DataConsistencyCheck.Tests
{
    [TestFixture]
    public class DataConsistencyTests
    {
        private IDataConsistencyService _dataConsistencyService;

        [SetUp]
        public void SetUp()
        {
            var httpClient = new HttpClient();
            _dataConsistencyService = new DataConsistencyService(httpClient);
        }

        [Test]
        public async Task VerifyOrderDataConsistency()
        {
            var result = await _dataConsistencyService.CheckOrderDataConsistency();
            ClassicAssert.IsTrue(result, "Order data consistency check failed.");
        }

        [Test]
        public async Task VerifyProductDataConsistency()
        {
            var result = await _dataConsistencyService.CheckProductDataConsistency();
            ClassicAssert.IsTrue(result, "Product data consistency check failed.");
        }
    }
}
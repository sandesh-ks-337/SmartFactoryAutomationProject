using System.Net.Http;
using System.Threading.Tasks;

namespace SmartFactory_DataConsistencyCheck.Services
{
    public class DataConsistencyService : IDataConsistencyService
    {
        private readonly HttpClient _httpClient;

        public DataConsistencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckOrderDataConsistency()
        {
            var response = await _httpClient.GetAsync("http://localhost:7208/api/orders");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckProductDataConsistency()
        {
            var response = await _httpClient.GetAsync("http://localhost:7208/api/products");
            return response.IsSuccessStatusCode;
        }
    }
}
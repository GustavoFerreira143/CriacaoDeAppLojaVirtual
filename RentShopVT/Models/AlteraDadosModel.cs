using System.Text;
using System.Text.Json;

namespace RentShopVT.Models;

    public class AlteraDadosModel
    {
        private readonly HttpClient _httpClient;

        public AlteraDadosModel()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<HttpResponseMessage> EnviaTroca()
        {

            return null;
        }
    }

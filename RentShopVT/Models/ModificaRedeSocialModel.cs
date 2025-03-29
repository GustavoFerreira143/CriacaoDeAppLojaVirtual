using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace RentShopVT.Models
{
    class ModificaRedeSocialModel
    {
        private readonly HttpClient _httpclient;
        public ModificaRedeSocialModel()
        {
            _httpclient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }
        public async Task<HttpResponseMessage> ModificaRedeSocial(Dictionary<string,List<string>> json)
        {
            try
            {

                var dados = new
                {
                    RedesSociais = json
                };
                var conteudo = JsonSerializer.Serialize(dados);
                var content = new StringContent(conteudo, Encoding.UTF8, "application/json");
                string token = Preferences.Get("Token","");
                _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpclient.PostAsync("http://192.168.100.63:5098/api/salvaredes", content);
                if(response.IsSuccessStatusCode)
                {
                    return response;
                }
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}

using System;
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
                long id = 0;

                long userId = Preferences.Get("Id", id);

                string ID = userId.ToString();

                var dados = new
                {
                    Id = ID,
                    RedesSociais = json
                };
                
                var conteudo = JsonSerializer.Serialize(dados);
                var content = new StringContent(conteudo, Encoding.UTF8, "application/json");

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

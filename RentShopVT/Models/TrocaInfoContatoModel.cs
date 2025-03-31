using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RentShopVT.Models
{
    class TrocaInfoContatoModel
    {
        private readonly HttpClient _httpClient;
        public TrocaInfoContatoModel()
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }
        public async Task<bool> TrocaContato(string coluna, string valor)
        {
            try
            {
                var usuario = new
                {
                    Coluna = coluna,
                    Valor = valor,
                };
                var json = JsonSerializer.Serialize(usuario);
                var conteudo = new StringContent(json, Encoding.UTF8, "application/json");
                string token = Preferences.Get("Token", "");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await _httpClient.PostAsync($"{Config.MeuUrl}api/TrocaContatoEmail", conteudo);

                response.EnsureSuccessStatusCode();
                string resposta = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                return false;
            }
        }
    }
}

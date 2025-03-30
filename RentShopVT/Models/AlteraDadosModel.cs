using Microsoft.Maui.ApplicationModel.Communication;
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

    public async Task<HttpResponseMessage> EnviaTroca(string email, string senha)
    {
        try
        {
            var Dados = new
            {
                Email = email,
                Senha = senha,
                HashSecreto = Config.MeuTokenTrocaSenha
            };
            var json = JsonSerializer.Serialize(Dados);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{Config.MeuUrl}api/trocasenha", content);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            else
            {
                // Debug: Verificar status code e conteúdo da resposta
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro: {response.StatusCode} - {response.ReasonPhrase}");
                Console.WriteLine($"Detalhes da resposta: {responseContent}");

                return null;
            }
        }
        catch (Exception ex)
        {
            return null;
        }

    }

}


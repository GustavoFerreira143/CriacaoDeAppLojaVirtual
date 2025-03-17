using System;
using RestSharp; // RestSharp v112.1.0
using RestSharp.Authenticators;
using System.Threading;
using System.Threading.Tasks;


namespace RentShopVT.Models;
public static class EmailService
{
    // Método para gerar o token de 6 caracteres
    private static string GerarToken()
    {
        var random = new Random();
        var caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Range(0, 6)
            .Select(_ => caracteres[random.Next(caracteres.Length)])
            .ToArray());
    }

    // Método para enviar o e-mail com o token
    public static async Task<RestResponse> Send(string emailDestinatario)
    {


        var apiKey = Config.ApiKey;

        string token = GerarToken();

        var options = new RestClientOptions("https://api.mailgun.net/v3")
        {
            Authenticator = new HttpBasicAuthenticator("api", apiKey)
        };

        var client = new RestClient(options);
        var request = new RestRequest("/sandbox501e2e8780b54bb0a16438b60ce7b034.mailgun.org/messages", Method.Post);

        request.AlwaysMultipartFormData = true;

        // Adiciona os parâmetros do e-mail
        request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox501e2e8780b54bb0a16438b60ce7b034.mailgun.org>");
        request.AddParameter("to", emailDestinatario);  // Recebe o e-mail do usuário
        request.AddParameter("subject", "Seu Código de Verificação");
        request.AddParameter("text", $"Seu código de verificação é: {token}");

        // Envia o e-mail e retorna a resposta
        return await client.ExecuteAsync(request);
    }
}

using Azure;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;


namespace RentShopVT.Models
{
    class EnvioDeImagensModel
    {
        private readonly HttpClient _httpClient;
        public EnvioDeImagensModel()
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }
        public async Task<Retorno> EnviarImagemAsync(FileResult foto)
        {
            try
            {
                if (foto == null)
                    return new Retorno { Status = "400",
                                          Link = "Erro Nenhuma imagem Selecionada"}; 
               
                string extensao = Path.GetExtension(foto.FileName).ToLower();
                string mimeType = extensao switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => null
                };
                
                if (mimeType == null)
                    return new Retorno { Status="400", Link = "Arquivo Inválido Enviado" }; 

                long id = 0;

                long userId = Preferences.Get("Id", id);

                string ID = userId.ToString();

                if (string.IsNullOrEmpty(ID) || ID == "0")
                    return new Retorno { Status = "400", Link = "Usuário deve Estar Logado" };

                using var stream = await foto.OpenReadAsync();
                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(stream);

                // Define o tipo de conteúdo correto
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                content.Add(streamContent, "imagem", foto.FileName);
                content.Add(new StringContent(ID), "\"ID\"");
                string token = Preferences.Get("Token", "");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var resposta = await _httpClient.PostAsync("http://192.168.100.63:5098/api/uploadimagemperfil", content);

                string respostaJson = await resposta.Content.ReadAsStringAsync();

                var retornoBanco = JsonSerializer.Deserialize<Retorno>(respostaJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (retornoBanco != null)
                {
                    return retornoBanco;
                }
                else
                {
                    return new Retorno
                    {
                        Status = "400",
                        Link = "Erro no Envio da imagem"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar imagem: {ex.Message}");
                return new Retorno
                {
                    Status = "400",
                    Link = "Erro no envio da Imagem"
                };
            }
        }

    }
    public class Retorno
    {
        public string Status { get; set; }
        public string Link { get; set; }
    }
}

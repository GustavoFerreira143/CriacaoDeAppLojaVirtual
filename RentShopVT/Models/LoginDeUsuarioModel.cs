using System.Text;
using System.Text.Json;

namespace RentShopVT.Models
{
    public class LoginDeUsuarioModel
    {
        private readonly HttpClient _httpClient;

        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string? NomeEmpresa { get; set; }
        public string? CNPJ { get; set; }
        public string? CPF { get; set; }
        public bool AutorizadoVenda { get; set; }
        public string? FotoPerfil { get; set; }
        public string? Contato { get; set; }
        public string Token { get; set; }
        public Dictionary<string, List<string>>? RedesSociais { get; set; }
        public bool? Success { get; set; }
        public string? Message { get; set; }

        public LoginDeUsuarioModel()
        {
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<LoginDeUsuarioModel> ConferirEmail(string email, string senha)
        {
            try
            {
                var usuario = new
                {
                    Email = email,
                    Senha = senha,
                    MeuHashSecreto = Config.MeuTokenLoginUser
                };

                string json = JsonSerializer.Serialize(usuario);
               
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                HttpResponseMessage response = await _httpClient.PostAsync($"{Config.MeuUrl}api/VerificarLogin", content);
               
                if (!response.IsSuccessStatusCode)
                {
                    return new LoginDeUsuarioModel
                    {
                        Success = false,
                        Message = $"Email ou Senha Inválidos"
                    };
                }
                
                string respostaJson = await response.Content.ReadAsStringAsync();

                var listaUsuarios = JsonSerializer.Deserialize<LoginDeUsuarioModel>(respostaJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if(listaUsuarios != null)
                {
                    return listaUsuarios;
                }
                else
                {
                    return new LoginDeUsuarioModel { Success = false, Message = "Usuário não encontrado" };
                }
                 
            }
            catch (HttpRequestException ex)
            {
                
                return new LoginDeUsuarioModel { Success = false, Message = $"Erro na requisição HTTP: {ex.Message}" };
            }
            catch (JsonException ex)
            {
               
                return new LoginDeUsuarioModel { Success = false, Message = $"Erro na desserialização JSON: {ex.Message}" };
            }
            catch (Exception ex)
            {

                return new LoginDeUsuarioModel { Success = false, Message = $"Erro inesperado: {ex.Message}" };
            }
        }
    }

}
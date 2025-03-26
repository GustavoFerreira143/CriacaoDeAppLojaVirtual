using System.Text;
using System.Text.Json;


namespace RentShopVT.Models
{
    public class RecebeCadastroModel
    {
        private readonly HttpClient _httpClient;

        public RecebeCadastroModel()
        {
            _httpClient = new HttpClient();
        }

        public async Task<bool> InserirUsuario(string nome, string email,string telefone, string senha, string cnpj, string nomeEmpresa, string cpf, bool termos)
        {
            try
            {
                var usuario = new
                {
                    Nome = nome,
                    Email = email,
                    Contato = string.IsNullOrEmpty(telefone) ? null : telefone,
                    Senha = senha,
                    CNPJ = string.IsNullOrEmpty(cnpj) ? null : cnpj,
                    NomeEmpresa = string.IsNullOrEmpty(nomeEmpresa) ? null : nomeEmpresa,
                    CPF = string.IsNullOrEmpty(cpf) ? null : cpf,
                    AutorizadoVenda = termos
                };

                string json = JsonSerializer.Serialize(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("http://192.168.100.63:5098/api/InserirValores", content);
                response.EnsureSuccessStatusCode();
                return true;

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Erro na requisição: {ex.Message}");
                return false;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro na serialização JSON: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cadastrar usuário: {ex.Message}");
                return false;
            }
        }
    }
}
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RentShopVT.Models
{
    class VerificaDuplicidade
    {
        private readonly HttpClient _httpClient;

        public VerificaDuplicidade()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public async Task<ResultadoOperacao> VerifiqueDuplicidades(string Coluna, string Valor)
        {
            try
            {
                var usuario = new
                {
                    Coluna = Coluna,
                    Valor = Valor,
                };
                var json = JsonSerializer.Serialize(usuario);
                var conteudo = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("http://192.168.100.63:5098/api/VerificarDuplicidade", conteudo);

                response.EnsureSuccessStatusCode();
                string resposta = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return new ResultadoOperacao
                    {
                        Success = true,
                        Message = "Valor Duplicado Encontrado"
                    };
                }
                else
                {
                    return new ResultadoOperacao
                    {
                        Success = false,
                        Message = "Autorizado"
                    };
                }
            }
            catch (TaskCanceledException ex) when (!ex.CancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Erro: A requisição demorou muito tempo e foi cancelada (Timeout).");
                return new ResultadoOperacao
                {
                    Success = true,
                    Message = "Servidor Demorou muito para Responder"
                };
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Erro 404: Recurso não encontrado - {ex.Message}");
                return new ResultadoOperacao
                {
                    Success = false,
                    Message = "Não encontrado"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                return new ResultadoOperacao
                {
                    Success = true,
                    Message = "Erro não esperado, tente novamente"
                };
            }
        }
    }
    public class ResultadoOperacao
    {
        public bool Success { get; set; } 
        public string Message { get; set; } 
    }
}

using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using RentShopVT.Models;
using RentShopVT.Views.Components;

namespace RentShopVT.ViewModels
{
    public partial class RecuperaSenhaViewModel : ObservableObject
    {
        //-----------------------------------------------------------------------------------------Valores Email----------------------------------------------------------------------------------------
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string novaSenha;

        [ObservableProperty]
        private string confirmaSenha;

        [ObservableProperty]
        private bool viewEmail;

        [ObservableProperty]
        private bool viewToken;

        [ObservableProperty]
        private bool viewTrocaSenha;

        [ObservableProperty]
        private string emailDigitado;

        public string mensagem { get; set; }
        public ICommand EnviarCommand { get; }

        public ICommand EnviarToken { get; }

        public ICommand RetornaEmailCommand { get; }

        public ICommand EnviarTrocaDeSenha { get; }

        private INavigation navigation;

        public RecuperaSenhaViewModel(INavigation _navigation)
        {
            EmailDigitado = "Nenhum Email Digitado";

            navigation = _navigation;
            ViewEmail = true;
            ViewToken = false;
            ViewTrocaSenha = false;

            EnviarCommand = new RelayCommand(async () => await Enviar());

            EnviarToken = new RelayCommand(async () => await Token());

            RetornaEmailCommand = new RelayCommand(async () => await RetornaEmail());

            EnviarTrocaDeSenha = new RelayCommand(async () => await EnviaTrocadeSenhas());
        }

        //------------------------------------------------------------------------------------------Envia Token por Email--------------------------------------------------------------------------------
        private async Task Enviar()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "O Campo está vázio", "Red"));
                return;
            }

            try
            {
                var popup = new TelaLoading();

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    MopupService.Instance.PushAsync(popup);
                });

                VerificaDuplicidade duplicidade = new VerificaDuplicidade();

                ResultadoOperacao Verificacao = await duplicidade.VerifiqueDuplicidades("Email", Email);
                if (Verificacao.Success == true && Verificacao.Message == "Valor Duplicado Encontrado")
                {
                    var response = await EmailService.Send(Email);

                    if (response.IsSuccessStatusCode)
                    {
                        
                        string content = await response.Content.ReadAsStringAsync();
                        var mensagem = JsonSerializer.Deserialize<Retorno>(content);

                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            MopupService.Instance.PopAsync();
                        });

                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", "E-Mail Enviado com Sucesso seu token é " + mensagem.mensagem, "Green"));

                        ViewEmail = false;
                        ViewToken = true;
                        ViewTrocaSenha = false;

                        EmailDigitado = Email;
                        Email = "";
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao enviar e-mail: {response.StatusCode}");
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "E-Mail Não Encontrado", "Red"));
                        ViewToken = false;
                        ViewTrocaSenha = false;
                    }
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        MopupService.Instance.PopAsync();
                    });

                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "E-Mail Não Consta no Banco de Dados", "Red"));
                }
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    MopupService.Instance.PopAsync();
                });
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Houve Problemas Internos Tente Novamente", "Red"));
                ViewToken = false;
                ViewTrocaSenha = false;
            }
        }

        //--------------------------------------------------------------------------------------------Retorna para Email---------------------------------------------------------------------------------

        private async Task RetornaEmail()
        {
            ViewEmail = true;
            ViewToken = false;
            ViewTrocaSenha = false;
        }


        //------------------------------------------------------------------------------------------Compará Token Recebido--------------------------------------------------------------------------------
        [ObservableProperty]
        private string valorToken;

        private async Task Token()
        {
            if (string.IsNullOrWhiteSpace(ValorToken))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "O Campo token está vázio", "Red"));
                return;
            }
            try
            {
                if (Preferences.Get("TokenRecuperacao","") == ValorToken)
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", "Token Correto Inserido", "Green"));
                    ViewEmail = false;
                    ViewToken = false;
                    ViewTrocaSenha = true;
                    ValorToken = "";
                }
                else
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Token Incorreto Inserido", "Red"));
                    ViewEmail = false;
                    ViewToken = true;
                    ViewTrocaSenha = false;
                    ValorToken = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Houve Problemas Internos Tente Novamente", "Red"));
                ViewEmail = true;
                ViewToken = false;
                ViewTrocaSenha = false;
                ValorToken = "";
            }
        }

        private async Task EnviaTrocadeSenhas()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NovaSenha) && string.IsNullOrWhiteSpace(ConfirmaSenha))
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Há Campos vázios", "Red"));
                    return;
                }
                if (NovaSenha != ConfirmaSenha)
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "As Senhas Não Batem", "Red"));
                    return;
                }
                if (NovaSenha.Length < 6)
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "A Senha Deve Ter no Minimo 6 Caracteres", "Red"));
                    return;
                }
                if (!SenhaPossuiCaractereEspecial(NovaSenha))
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "A Senha Deve Ter no Minimo 1 Caracter Especial", "Red"));
                    return;
                }
                var popup = new TelaLoading();

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    MopupService.Instance.PushAsync(popup);
                });

                AlteraDadosModel alteramodel = new AlteraDadosModel();
                var resposta = await alteramodel.EnviaTroca(EmailDigitado, NovaSenha);
                if (resposta != null )
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        MopupService.Instance.PopAsync();
                    });
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", "Sua Senha foi Alterada com Sucesso", "Green"));
                    navigation.PopModalAsync();
                    return;

                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        MopupService.Instance.PopAsync();

                    });
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "A Senha Não Pode ser Alterada Tente Novamente", "Red"));
                    return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                return;
            }

        }
        private bool SenhaPossuiCaractereEspecial(string senha)
        {
            return senha.Any(ch => !char.IsLetterOrDigit(ch));
        }
           public class Retorno
        {
            public string mensagem { get; set; }
        }
    }
}

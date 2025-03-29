using System.Text.RegularExpressions;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentShopVT.Views.Components;
using Mopups.Services;
using RentShopVT.Models;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Text.Json;

namespace RentShopVT.ViewModels
{
    public partial class LoginDeUsuarioViewModel : ObservableObject
    {
        private INavigation _navigation;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string senhaUser;

        public ICommand EnviarLogin { get; }
        public LoginDeUsuarioViewModel(INavigation navigation)
        {
            _navigation = navigation; 
            Email = "";
            SenhaUser = "";
            EnviarLogin = new RelayCommand(async () => await EnviarLoginUser());
        }

        private async Task EnviarLoginUser()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(SenhaUser))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro no Envio", "Há Campos Não Preenchidos", "Red"));
                return;
            }
            if(!EmailValido(Email))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro no Envio", "Email Inválido", "Red"));
                return;
            }
               try
                {
                    VerificaDuplicidade duplicidade = new VerificaDuplicidade();

                var popup = new TelaLoading();

                await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        MopupService.Instance.PushAsync(popup);
                    });


                var verificacao = new LoginDeUsuarioModel();

                var response = await verificacao.ConferirEmail(Email, SenhaUser);
                

                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (MopupService.Instance.PopupStack.Contains(popup))
                        MopupService.Instance.PopAsync();
                });

                if (response.Success == false)
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", response.Message , "Red"));
                    return;
                }

                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", $"Usuário Vinculado com Sucesso", "Green"));
                var json = JsonSerializer.Serialize(response.RedesSociais);

                //--------------------------------------------------------------------------------------Salvando Informações do Usuário-----------------------------------------------------------------------------------
                Preferences.Set("Id", response.Id);
                Preferences.Set("Nome", response.Nome);
                Preferences.Set("Email", response.Email);
                Preferences.Set("NomeEmpresa", response.NomeEmpresa);
                Preferences.Set("CNPJ", response.CNPJ);
                Preferences.Set("CPF", response.CPF);
                Preferences.Set("AutorizadoVenda", response.AutorizadoVenda);
                Preferences.Set("FotoPerfil", "http://192.168.100.63:5098" + response.FotoPerfil);
                Preferences.Set("TelefoneUser", response.Contato);
                Preferences.Set("UsuarioLogado", true);
                Preferences.Set("RedesSociais", json);
                Preferences.Set("Token", response.Token);

                await _navigation.PopModalAsync();


            }
            catch (Exception ex)
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro no Envio", ex.Message, "Red"));
                }
            }

        private bool EmailValido(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}

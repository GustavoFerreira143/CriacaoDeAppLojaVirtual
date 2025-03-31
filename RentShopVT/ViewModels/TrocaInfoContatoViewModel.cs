using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using System.Windows.Input;
using RentShopVT.Views.Components;
using Mopups.Services;
using RentShopVT.Models;


namespace RentShopVT.ViewModels
{
    partial class TrocaInfoContatoViewModel : ObservableObject
    {
        public INavigation _navigation;
        public ICommand EnviaContato { get; }
        public ICommand EnviaEmail { get; }

        [ObservableProperty]
        private string emailUser;

        [ObservableProperty]
        private string contato;
        public TrocaInfoContatoViewModel(INavigation navigation)
        {
            _navigation = navigation;
            EmailUser = Preferences.Get("Email", "");
            Contato = Preferences.Get("TelefoneUser", "");
            EnviaEmail = new RelayCommand(async () => await TrocaEmail());
            EnviaContato = new RelayCommand(async () => await TrocaContato());

        }
        public async Task TrocaEmail()
        {
            if(string.IsNullOrEmpty(EmailUser))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "O Campo está vázio", "Red"));
                return;
            }
            if(!EmailValido(EmailUser))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Email Inválido Detectado", "Red"));
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

                ResultadoOperacao Verificacao = await duplicidade.VerifiqueDuplicidades("Email", EmailUser);
                if (Verificacao.Success == true)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        MopupService.Instance.PopAsync();
                    });

                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "E-Mail Inserido Já Existe", "Red"));
                    return;
                }
                else
                    {
                    TrocaInfoContatoModel resposta = new TrocaInfoContatoModel();
                    bool result = await resposta.TrocaContato("Email", EmailUser);
                    if (result)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            MopupService.Instance.PopAsync();
                        });
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", "Email Inserido Com Sucesso", "Green"));
                        Preferences.Set("Email", EmailUser);
                        await _navigation.PopModalAsync();
                        return;
                    }
                    else
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            MopupService.Instance.PopAsync();
                        });
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Houve um Erro Interno Tente Novamente", "Red"));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    MopupService.Instance.PopAsync();
                });
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Houve Problemas Internos Tente Novamente", "Red"));
                return;
            }
        }
        private bool EmailValido(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
        public async Task TrocaContato()
        {
            if (string.IsNullOrEmpty(Contato))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "O Campo está vázio", "Red"));
                return;
            }
            if (!TelefoneValido(Contato,10))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Telefone Inválido Detectado", "Red"));
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

                ResultadoOperacao Verificacao = await duplicidade.VerifiqueDuplicidades("Contato", Contato);
                if (Verificacao.Success == true)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        MopupService.Instance.PopAsync();
                    });

                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Telefone Inserido Já Existe", "Red"));
                    return;
                }
                else
                {
                    TrocaInfoContatoModel resposta = new TrocaInfoContatoModel();
                    bool result = await resposta.TrocaContato("Contato",Contato);
                    if(result)
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            MopupService.Instance.PopAsync();
                        });
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", "Telefone Inserido Com Sucesso", "Green"));
                        Preferences.Set("TelefoneUser", Contato);
                        await _navigation.PopModalAsync();
                        return;
                    }
                    else
                    {
                        await MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            MopupService.Instance.PopAsync();
                        });
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Houve um Erro Interno Tente Novamente", "Red"));
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    MopupService.Instance.PopAsync();
                });
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Houve Problemas Internos Tente Novamente", "Red"));
                return;
            }
        }
        private bool TelefoneValido(string telefone, int minimoCaracteres)
        {
            return telefone.Length >= minimoCaracteres && telefone.All(char.IsDigit);
        }
    }
}

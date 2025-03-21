using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentShopVT.Models;
using RentShopVT.Views.Components;

namespace RentShopVT.ViewModels
{
    public partial class CadastrarUserViewModel : ObservableObject
    {
//---------------------------------------------------------------------------------------Dados da Página------------------------------------------------------------------------------------------------
        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string emailEnviado;

        [ObservableProperty]
        private string token;

        [ObservableProperty]
        private bool termos;

        [ObservableProperty]
        private bool termosAceitos;

        [ObservableProperty]
        private string nomeEmpresa;

        [ObservableProperty]
        private string cnpj;

        [ObservableProperty]
        private string cpf;

        [ObservableProperty]
        private string senha;

        [ObservableProperty]
        private string confirmarSenha;

//----------------------------------------------------------------------------------------Controle de Views da Página------------------------------------------------------------------------------------

        [ObservableProperty]
        private bool viewPrimeiraParte;

        [ObservableProperty]
        private bool viewSegundaParte;

        [ObservableProperty]
        private bool viewTerceiraParte;

        [ObservableProperty]
        private bool viewQuartaParte;

        [ObservableProperty]
        private bool viewQuintaParte;
        
        [ObservableProperty]
        private bool viewFinalCnpj;

        [ObservableProperty]
        private bool viewFinalCpf;


//--------------------------------------------------------------------------------Controle de Eventos de CLick da Página----------------------------------------------------------------------------

        public ICommand EnviarPrimeiraEtapaCadastro { get; }

        public ICommand EnviarSegundaEtapaCadastro { get; }

        public ICommand EnviarTerceiraEtapaCadastro { get; }

        public ICommand EnviarTipoCadastroCnpj { get; }

        public ICommand EnviarTipoCadastroCpf { get; }

        public ICommand EnviarUltimaEtapaCadastro { get; }

        public CadastrarUserViewModel()
        {
            ViewPrimeiraParte = true;
            ViewSegundaParte = false;
            ViewTerceiraParte = false;
            ViewQuartaParte = false;
            ViewQuintaParte = false;
            ViewFinalCnpj = false;
            ViewFinalCpf = false;

            EnviarPrimeiraEtapaCadastro = new RelayCommand(async () => await PrimeiraEtapa());

            EnviarSegundaEtapaCadastro = new RelayCommand(async () => await SegundaEtapa());

            EnviarTerceiraEtapaCadastro = new RelayCommand(async () => await TerceiraEtapa());

            EnviarTipoCadastroCnpj = new RelayCommand(async () => await TipoCnpj());

            EnviarTipoCadastroCpf = new RelayCommand(async () => await TipoCpf());

            EnviarUltimaEtapaCadastro = new RelayCommand(async () => await EnviarDados());
        }

//--------------------------------------------------------------------------------------------Verifica Campos Email e Nome e da Poceguimento-------------------------------------------------------------
        private async Task PrimeiraEtapa()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Nome))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Há campos não preenchidos", "Red"));
                return;
            }

            if (!EmailValido(Email))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Email inválido. Verifique o formato.", "Red"));
                return;
            }
            try
            {
                var response = await EmailService.Send(Email);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var mensagem = JsonSerializer.Deserialize<RecuperaSenhaViewModel>(content);

                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", $"Seu Token: {mensagem.mensagem}", "Green"));
                    EmailEnviado = Email;
                    ViewPrimeiraParte = false;
                    ViewSegundaParte = true;
                }
                else
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Falha ao enviar e-mail", "Red"));
                }
            }
            catch(Exception e)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Erro Interno Tente Novamente"+e , "Red"));
            }
        }

        private bool EmailValido(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        //--------------------------------------------------------------------------------------------Verifica Campo Token e da Poceguimento-------------------------------------------------------------
        private async Task SegundaEtapa()
        {
            if (string.IsNullOrWhiteSpace(Token))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "O Campo está vázio", "Red"));
                return;
            }
            if(Preferences.Get("TokenRecuperacao","") == Token)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", "Token Correto Inserido", "Green"));
                ViewSegundaParte = false;
                ViewTerceiraParte = true;
            }
            else
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Token Inválido", "Red"));
            }

        }
        partial void OnTermosChanged(bool value)
        {
            if (value)
            {
                TermosAceitos = true; 
            }
            else
            {
                TermosAceitos = false;
            }
        }

        private async Task TerceiraEtapa()
        {
            ViewTerceiraParte = false;
            ViewQuartaParte = true;
        }
        private async Task TipoCnpj()
        {
            ViewQuartaParte = false;
            ViewQuintaParte = true;
            ViewFinalCnpj = true;
        }
        private async Task TipoCpf()
        {
            ViewQuartaParte = false;
            ViewQuintaParte = true;
            ViewFinalCpf = true;
        }
        private async Task EnviarDados()
        {
            
        }

    }
}

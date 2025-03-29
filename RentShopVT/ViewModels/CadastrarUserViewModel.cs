using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using RentShopVT.Models;
using RentShopVT.Views.Components;

namespace RentShopVT.ViewModels
{
    public partial class CadastrarUserViewModel : ObservableObject
    {
        private INavigation _navigation;

        //---------------------------------------------------------------------------------------Dados da Página------------------------------------------------------------------------------------------------
        [ObservableProperty]
        private string nome;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string emailEnviado;

        [ObservableProperty]
        private string telefone;

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

        public CadastrarUserViewModel(INavigation navigation)
        {
            _navigation = navigation;

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
            if(!string.IsNullOrWhiteSpace(Telefone))
            {
                if(telefone.Length < 10)
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Telefone Inserido de Forma Incorreta", "Red"));
                    return;
                }
            }
            if (!EmailValido(Email))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Email inválido. Verifique o formato.", "Red"));
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


                ResultadoOperacao Verificacao = await duplicidade.VerifiqueDuplicidades("Email", Email);


                if (Verificacao.Success)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        if (MopupService.Instance.PopupStack.Contains(popup))
                             MopupService.Instance.PopAsync();
                    });
                    if (Verificacao.Message == "Valor Duplicado Encontrado")
                    {
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro de Duplicidade", "O Email Inserido já existe.", "Red"));
                        return;
                    }
                    else
                    {
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", Verificacao.Message, "Red"));
                        return;
                    }
                }
                var response = await EmailService.Send(Email);

                if (response.IsSuccessStatusCode)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        if (MopupService.Instance.PopupStack.Contains(popup))
                            MopupService.Instance.PopAsync();
                    });

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
//----------------------------------------------------------------Tratamento de Campos Durante Envio-------------------------------------------------------------------------

            if (string.IsNullOrWhiteSpace(Senha) || string.IsNullOrWhiteSpace(ConfirmarSenha))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "A senha e a confirmação não podem estar vazias!", "Red"));
                return;
            }

            if (Senha.Length < 6)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "A senha deve ter no mínimo 6 caracteres!", "Red"));
                return;
            }

            if (!SenhaPossuiCaractereEspecial(Senha))
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "A senha deve conter pelo menos um caractere especial!", "Red"));
                return;
            }

            if (Senha != ConfirmarSenha)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "As senhas não coincidem!", "Red"));
                return;
            }

            bool temCnpj = !string.IsNullOrWhiteSpace(Cnpj) && !string.IsNullOrWhiteSpace(NomeEmpresa);
            bool temCpf = !string.IsNullOrWhiteSpace(Cpf);

            if (temCnpj && temCpf)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Informe apenas CNPJ ou CPF, nunca ambos!", "Red"));
                return;
            }

            if (temCnpj && Cnpj.Length != 14)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "CNPJ inválido! Deve ter 14 dígitos.", "Red"));
                return;
            }

            if (temCpf && Cpf.Length != 11)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "CPF inválido! Deve ter 11 dígitos.", "Red"));
                return;
            }

            if (!Termos)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Você deve aceitar os termos para continuar!", "Red"));
                return;
            }
            var popup = new TelaLoading();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                MopupService.Instance.PushAsync(popup);
            });

            try
            {
            VerificaDuplicidade duplicidade = new VerificaDuplicidade();

            if (!string.IsNullOrWhiteSpace(Cpf) && Cpf.Length == 11)
            {
                    ResultadoOperacao Verificacao = await duplicidade.VerifiqueDuplicidades("CPF", Cpf);
                if(Verificacao.Success)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        if (MopupService.Instance.PopupStack.Contains(popup))
                            MopupService.Instance.PopAsync();
                    });
                        if (Verificacao.Message == "Valor Duplicado Encontrado")
                        {
                            Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro de Duplicidade", "O CPF Inserido já existe", "Red"));
                            return;
                        }
                        else
                        {
                            Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", Verificacao.Message, "Red"));
                            return;
                        }

                }
                   if(!ValidarCPF(Cpf))
                    {
                        if (MopupService.Instance.PopupStack.Contains(popup))
                            MopupService.Instance.PopAsync();

                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "CPF Inválido Detectado", "Red"));
                        return;
                    }
            }
            if(!string.IsNullOrWhiteSpace(Cnpj) && Cnpj.Length == 14)
            {
               ResultadoOperacao Verificacao = await duplicidade.VerifiqueDuplicidades("CNPJ", Cnpj);
                if (Verificacao.Success)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        if (MopupService.Instance.PopupStack.Contains(popup))
                            MopupService.Instance.PopAsync();
                    });
                        if (Verificacao.Message == "Valor Duplicado Encontrado")
                        {
                            Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro de Duplicidade", "O CNPJ Inserido já existe", "Red"));
                            return;
                        }
                        else
                        {
                            Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", Verificacao.Message, "Red"));
                            return;
                        }
                    }
                    if (!ValidarCNPJ(Cnpj))
                    {
                        if (MopupService.Instance.PopupStack.Contains(popup))
                            MopupService.Instance.PopAsync();

                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "CNPJ Inválido Detectado", "Red"));
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (MopupService.Instance.PopupStack.Contains(popup))
                        MopupService.Instance.PopAsync();
                });
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro de consulta", "Houve erro no momento de verificação Tente Novamente", "Red"));
                return;
            }

            //----------------------------------------------------------------Fim Tratamento de Campos Durante Envio-------------------------------------------------------------------------


            //-----------------------------------------------------------------Envio dos Dados para Armazenamento----------------------------------------------------------------------------

            RecebeCadastroModel userRepository = new RecebeCadastroModel();
            bool resultado = await userRepository.InserirUsuario(Nome, Email, Telefone, Senha, Cnpj, NomeEmpresa, Cpf, Termos);

            if (resultado)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (MopupService.Instance.PopupStack.Contains(popup))
                        MopupService.Instance.PopAsync();
                });

                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", "Cadastro realizado com sucesso!", "Green"));

                await _navigation.PopModalAsync();
            }
            else
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (MopupService.Instance.PopupStack.Contains(popup))
                        MopupService.Instance.PopAsync();
                });

                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Erro ao salvar no banco!", "Red"));
            }
        }

        private bool SenhaPossuiCaractereEspecial(string senha)
        {
            return senha.Any(ch => !char.IsLetterOrDigit(ch));
        }
        public static bool ValidarCPF(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            // Verifica se o CPF tem 11 caracteres
            if (cpf.Length != 11) return false;

            // CPF não pode ser uma sequência de números repetidos
            if (cpf.Distinct().Count() == 1) return false;

            // Cálculo do primeiro dígito verificador
            int soma = 0;
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = (resto < 2) ? 0 : 11 - resto;

            // Cálculo do segundo dígito verificador
            soma = 0;
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = (resto < 2) ? 0 : 11 - resto;

            // Compara os dois dígitos verificadores calculados com os fornecidos
            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }
        public static bool ValidarCNPJ(string cnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Trim();

            // Verifica se o CNPJ tem 14 caracteres
            if (cnpj.Length != 14) return false;

            // CNPJ não pode ser uma sequência de números repetidos
            if (cnpj.Distinct().Count() == 1) return false;

            // Cálculo do primeiro dígito verificador
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = (resto < 2) ? 0 : 11 - resto;

            // Cálculo do segundo dígito verificador
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digito2 = (resto < 2) ? 0 : 11 - resto;

            // Compara os dois dígitos verificadores calculados com os fornecidos
            return cnpj.EndsWith(digito1.ToString() + digito2.ToString());
        }
    }
}

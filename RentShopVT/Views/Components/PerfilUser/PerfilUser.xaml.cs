using CommunityToolkit.Maui.Views;
using Mopups.Services;
using RentShopVT.ViewModels;
using RentShopVT.Models;
using RentShopVT.Views.Components.PerfilUser.ModificarRedesDeUser;
using System.Text.Json;

namespace RentShopVT.Views.Components.PerfilUser;

public partial class PerfilUser : ContentPage
{
    private readonly ModificaRedeSocialViewModel _viewModel;
    public PerfilUser()
	{
		InitializeComponent();
        BindingContext = new PerfilDeUsuarioViewModel(); 
	}

    //--------------------------------------------------------------------------------------------Executa a Função quando a tela é exibida---------------------------------------------------------
    protected override void OnAppearing()
    {
        base.OnAppearing();
        bool usuarioEstaLogado = VerificaSessao();
        VerificaLogado(usuarioEstaLogado);
        if (usuarioEstaLogado)
        {
            string Nome = Preferences.Get("Nome", "");
            string Email = Preferences.Get("Email", "");
            string NomeEmpresa = Preferences.Get("NomeEmpresa", "Sem Empresa Especificada");
            string FotoPerfil = Preferences.Get("FotoPerfil", "personcicle.svg");
            string Telefone = Preferences.Get("TelefoneUser", "+00 00 00000-0000");

            //-----------------------------------------------------------------------------------------------------Adiciona Icone Redes Sociais-----------------------------------------------------------------------

            string json = Preferences.Get("RedesSociais", "");

            if (json != "" && json != "{}")
            {
                var objeto = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);

                if (objeto != null)
                {
                    RedesHorizontalStack.Children.Clear();

                    foreach (var item in objeto)
                    {
                        string nomeRedeSocial = item.Key;
                        List<string> dadosRede = item.Value;

                        VerticalStackLayout verticalLayout = new VerticalStackLayout
                        {
                            Spacing = 5,
                            HorizontalOptions = LayoutOptions.Center
                        };

                        ImageButton imageButton = new ImageButton
                        {
                            Source = dadosRede[0], 
                            WidthRequest = 50,
                            HeightRequest = 50,
                            Aspect = Aspect.Fill,
                        };

                        imageButton.Clicked += async (sender, e) =>
                        {
                            string url = dadosRede[1] + dadosRede[2]; 
                            await Launcher.OpenAsync(url); 
                        };


                        Label label = new Label
                        {
                            Text = nomeRedeSocial,
                            HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Colors.Black
                        };

 
                        verticalLayout.Children.Add(imageButton);
                        verticalLayout.Children.Add(label);

                        RedesHorizontalStack.Children.Add(verticalLayout);
                    }
                }
            }
            else
            {
                RedesHorizontalStack.Children.Clear();
                Label label = new Label
                {
                    Text = "Nada Adicionado",
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.Black,
                    WidthRequest = 300,
                    HeightRequest = 70,
                    FontSize = 15,
                    FontAttributes = FontAttributes.Bold,
                    VerticalTextAlignment = TextAlignment.Center
                };
                RedesHorizontalStack.Children.Add(label);
            }
            NomeUsuario.Text = Nome;
            EmailUser.Text = Email;
            NomeEmpresaUser.Text = NomeEmpresa;
            FotoPerfilUser.Source = FotoPerfil;
            TelefoneUser.Text = Telefone;
        }
        else
        {
            NomeUsuario.Text = "";
            NomeEmpresaUser.Text = "";
            FotoPerfilUser.Source = "personcicle.svg";
            TelefoneUser.Text = "";
            EmailUser.Text = "";
        }
    }
    //---------------------------------------------------------------------------------------------Verifique se Usuario Logado Altera View---------------------------------------------------------------------
    public void VerificaLogado(bool valor)
    {
        if (valor)
        {
            BotaoLoginLogout.Text = "Logout";
            Logado.IsVisible = true;
            NaoLogado.IsVisible = false;
        }
        else
        {
            BotaoLoginLogout.Text = "Login";
            Logado.IsVisible = false;
            NaoLogado.IsVisible = true;
        }
    }
    private bool VerificaSessao()
    {
        return Preferences.Get("UsuarioLogado", false);
    }
    //--------------------------------------------------------------------------------------------------------------Botão Login / Logout--------------------------------------------------------------
    async private void Button_Clicked(object sender, EventArgs e)
    {
        if (BotaoLoginLogout.Text == "Logout")
        {
            Preferences.Set("UsuarioLogado", false);
            await ProcessarLogout();
        }
        else
        {
            var novaPagina = new TelaLogin();

            novaPagina.TranslationX = 300;

            await Navigation.PushModalAsync(novaPagina, false);
            await Task.WhenAll(
                this.TranslateTo(-300, 0, 180, Easing.Linear),
                novaPagina.TranslateTo(0, 0, 180, Easing.Linear)
            );
            await this.TranslateTo(0, 0, 180, Easing.Linear);
        }
    }

    //----------------------------------------------------------------------------------------------------------Carregamento Login / Logout--------------------------------------------------------------

    private async Task ProcessarLogout()
    {
        try
        {
            var popup = new TelaLoading();
            await MopupService.Instance.PushAsync(popup);

            Preferences.Set("Id", "");
            Preferences.Set("Nome", "");
            Preferences.Set("Email", "");
            Preferences.Set("NomeEmpresa", "");
            Preferences.Set("CNPJ", "");
            Preferences.Set("CPF", "");
            Preferences.Set("AutorizadoVenda", false);
            Preferences.Set("FotoPerfil", "personcircle.svg");
            Preferences.Set("TelefoneUser", "+00 00 00000-0000");
            Preferences.Set("RedesSociais", "{}");
            Preferences.Set("Token", "");
            Preferences.Set("Linkedin", false);
            Preferences.Set("GitHub", false);
            Preferences.Set("Facebook", false);
            Preferences.Set("Twitter", false);
            Preferences.Set("WhatsApp", false);
            Preferences.Set("Tiktok", false);
            Preferences.Set("Youtube", false);
            Preferences.Set("Instagram", false);

            await Task.Delay(3000);

            await MopupService.Instance.PopAsync();

            var alerta = new CaixaDeAlerta("Sucesso", "Usuário Deslogado Com Sucesso", "Green");
            this.ShowPopup(alerta);

            VerificaLogado(false);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    //-----------------------------------------------------------------------------------------------------------Função Envio de Imagem Perfil--------------------------------------------------------------
    async private void EnviarFoto_Tapped(object sender, TappedEventArgs e)
    {
        EnvioDeImagensViewModel foto = new EnvioDeImagensViewModel();
       
        string imagem = await foto.SelecionarImagemAsync();

        if (imagem == "Sucesso")
        {
            string FotoPerfil = Preferences.Get("FotoPerfil", "personcicle.svg");
            FotoPerfilUser.Source = FotoPerfil;
        }
    }
//---------------------------------------------------------------------------------------------Abre Tela Editar Redes Sociais--------------------------------------------------------------------------------
    async private void AbreTelaRedes_Tapped(object sender, TappedEventArgs e)
    {

        await Navigation.PushModalAsync(new ModificaRedeSocial());
    }

    //------------------------------------------------------------------------------------------Evento Abre Telas Troca Contato e Email---------------------------------------------------------------------------
    async private void EditarContato_Clicked(object sender, EventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new TrocaInformacoesDeContato(0));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    async private void EditarEmail_Clicked(object sender, EventArgs e)
    {
            try
            {
                await MopupService.Instance.PushAsync(new TrocaInformacoesDeContato(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
    }
}
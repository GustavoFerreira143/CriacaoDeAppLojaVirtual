using CommunityToolkit.Maui.Views;
using Mopups.Services;
using RentShopVT.ViewModels;
using RentShopVT.Views.Components;
using RentShopVT.Views.Components.CarrinhoDeCompras;

namespace RentShopVT.Views.Components.PerfilUser;

public partial class PerfilUser : ContentPage
{
	public PerfilUser()
	{
		InitializeComponent();
	}

    //--------------------------------------------------------------------------------------------Executa a Função quando a tela é exibida---------------------------------------------------------
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("Disparou");
        bool usuarioEstaLogado = VerificaSessao();
        VerificaLogado(usuarioEstaLogado);
        if (usuarioEstaLogado)
        {
            string Nome = Preferences.Get("Nome", "");
            string Email = Preferences.Get("Email", "");
            string NomeEmpresa = Preferences.Get("NomeEmpresa", "Sem Empresa Especificada");
            string FotoPerfil = Preferences.Get("FotoPerfil", "personcicle.svg");
            string Telefone = Preferences.Get("TelefoneUser", "+00 00 00000-0000");
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

        await Task.Delay(3000);

        await MopupService.Instance.PopAsync();

        var alerta = new CaixaDeAlerta("Sucesso", "Usuário Deslogado Com Sucesso", "Green");
        this.ShowPopup(alerta);

        VerificaLogado(false);
    }

}
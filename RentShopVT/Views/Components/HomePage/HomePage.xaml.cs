
using Azure;
using CommunityToolkit.Maui.Views;
using Mopups.Services;

namespace RentShopVT.Views.Components.HomePage;

public partial class HomePage : ContentPage
{
	public HomePage()
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
    }
    //---------------------------------------------------------------------------------------------Verifique se Usuario Logado Altera View---------------------------------------------------------------------
    public void VerificaLogado(bool valor)
    {
        if (valor)
        {
            BotaoLoginLogout.Text = "Logout";
        }
        else
        {
            BotaoLoginLogout.Text = "Login";
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
        Preferences.Set("Nome","");
        Preferences.Set("Email","");
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
public class CarrousselItem
{
    public string? ImageSource { get; set; }
    public string? Description { get; set; }
    public string? Preco { get; set; }
}
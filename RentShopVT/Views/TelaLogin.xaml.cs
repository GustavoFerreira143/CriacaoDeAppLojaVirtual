using RentShopVT.Views.Components.HomePage;
using RentShopVT.Views.LoginComponents;
using RentShopVT.ViewModels;

namespace RentShopVT.Views;

public partial class TelaLogin : ContentPage
{
	public TelaLogin()
	{
		InitializeComponent();
        BindingContext = new LoginDeUsuarioViewModel(Navigation);

    }

    async private void Voltar_Clicked(object sender, EventArgs e)
    {

        var homePage = Application.Current.MainPage.Navigation.ModalStack.LastOrDefault();

        if (homePage != null)
        {
            // Anima a HomePage para voltar ao estado inicial
            var animHome = homePage.TranslateTo(0, 0, 180, Easing.Linear);

            // Aguarda a animação e então fecha o modal
            await Task.WhenAll(animHome, this.TranslateTo(350, 0, 180, Easing.Linear)) ;
            await Navigation.PopModalAsync(false);
        }
        else
        {
            await Navigation.PopModalAsync(false);
        }
    }

    async private void RecuperarSenha_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new RecuperarSenha());
    }

    async private void Cadastrar_User_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushModalAsync(new CadastrarUser());
    }


/*-------------------------------------------------------------------------Vizualizar e Esconder Senha--------------------------------------------------------------*/
    private void VizualizarSenha_Clicked(object sender, EventArgs e)
    {
        Senha.IsPassword = !Senha.IsPassword;

        if (Senha.IsPassword)
        {
            VizualizarSenha.Source = "eyeslashfill.svg";  
        }
        else
        {
            VizualizarSenha.Source = "eyefill.svg"; 
        }
    }
}

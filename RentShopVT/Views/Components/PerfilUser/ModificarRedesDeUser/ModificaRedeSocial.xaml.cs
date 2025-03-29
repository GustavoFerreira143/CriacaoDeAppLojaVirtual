using RentShopVT.ViewModels;
namespace RentShopVT.Views.Components.PerfilUser.ModificarRedesDeUser;

public partial class ModificaRedeSocial : ContentPage
{
	public ModificaRedeSocial()
	{
		InitializeComponent();
        BindingContext = new ModificaRedeSocialViewModel(Navigation);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ModificaRedeSocialViewModel modelo = new ModificaRedeSocialViewModel(Navigation);
        modelo.AtualizarRedesSociais();
    }

    async private void Voltar_Clicked(object sender, EventArgs e)
    {

        var homePage = Application.Current.MainPage.Navigation.ModalStack.LastOrDefault();

        if (homePage != null)
        {
            // Anima a HomePage para voltar ao estado inicial
            var animHome = homePage.TranslateTo(0, 0, 180, Easing.Linear);

            // Aguarda a animação e então fecha o modal
            await Task.WhenAll(animHome, this.TranslateTo(350, 0, 180, Easing.Linear));
            await Navigation.PopModalAsync(false);
        }
        else
        {
            await Navigation.PopModalAsync(false);
        }
    }
}
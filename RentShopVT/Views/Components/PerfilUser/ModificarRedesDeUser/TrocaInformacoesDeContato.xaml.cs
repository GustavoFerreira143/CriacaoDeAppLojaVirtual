using Mopups.Pages;
using Mopups.Services;
using RentShopVT.ViewModels;

namespace RentShopVT.Views.Components.PerfilUser.ModificarRedesDeUser;

public partial class TrocaInformacoesDeContato : ContentPage
{
	public TrocaInformacoesDeContato(int tela)
	{
		InitializeComponent();
		BindingContext = new TrocaInfoContatoViewModel(Navigation);
		if(tela == 0)
		{
			EmailUser.IsVisible = true;
			Contato.IsVisible = false;

        }
		else
		{
            EmailUser.IsVisible = false;
            Contato.IsVisible = true;
        }
	}
    async private void FecharPopup_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
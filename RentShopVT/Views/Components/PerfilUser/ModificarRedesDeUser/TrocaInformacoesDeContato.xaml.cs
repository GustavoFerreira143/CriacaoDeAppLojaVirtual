using Mopups.Pages;
using Mopups.Services;
using RentShopVT.ViewModels;

namespace RentShopVT.Views.Components.PerfilUser.ModificarRedesDeUser;

public partial class TrocaInformacoesDeContato : PopupPage
{
	public TrocaInformacoesDeContato(int tela)
	{
		InitializeComponent();
		BindingContext = new TrocaInformacoesDeContatoViewModel();
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
        await MopupService.Instance.PopAsync();
    }
}
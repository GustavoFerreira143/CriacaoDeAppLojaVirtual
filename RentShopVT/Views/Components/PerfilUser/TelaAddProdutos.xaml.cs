using System.Threading.Tasks;
using RentShopVT.ViewModels;

namespace RentShopVT.Views.Components.PerfilUser;

public partial class TelaAddProdutos : ContentPage
{
	public TelaAddProdutos()
	{
		InitializeComponent();
        BindingContext = new AdicionaProdutoViewModel();
	}

    private async void VoltaPerfil_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
using System.Threading.Tasks;

namespace RentShopVT.Views.Components.PerfilUser;

public partial class TelaAddProdutos : ContentPage
{
	public TelaAddProdutos()
	{
		InitializeComponent();
	}

    private async void VoltaPerfil_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
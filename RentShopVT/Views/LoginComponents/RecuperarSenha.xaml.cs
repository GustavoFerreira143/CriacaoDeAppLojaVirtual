using RentShopVT.ViewModels;
namespace RentShopVT.Views.LoginComponents;

public partial class RecuperarSenha : ContentPage
{
	public RecuperarSenha()
	{
		InitializeComponent();
        BindingContext = new RecuperaSenhaViewModel(); 
    }

    async private void VoltarLogin_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
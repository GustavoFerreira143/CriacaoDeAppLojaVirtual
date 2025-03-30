using RentShopVT.ViewModels;
namespace RentShopVT.Views.LoginComponents;

public partial class RecuperarSenha : ContentPage
{
	public RecuperarSenha()
	{
		InitializeComponent();
        BindingContext = new RecuperaSenhaViewModel(Navigation); 
    }

    async private void VoltarLogin_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private void VizualizarSenha_Clicked(object sender, EventArgs e)
    {
        InserirSenha.IsPassword = !InserirSenha.IsPassword;

        if (InserirSenha.IsPassword)
        {
            VizualizarSenha.Source = "eyeslashfill.svg";
        }
        else
        {
            VizualizarSenha.Source = "eyefill.svg";
        }
    }
    private void VizualizarSenhaConf_Clicked(object sender, EventArgs e)
    {
        ConfirmaSenha.IsPassword = !ConfirmaSenha.IsPassword;

        if (InserirSenha.IsPassword)
        {
            VizualizarSenhaConf.Source = "eyeslashfill.svg";
        }
        else
        {
            VizualizarSenhaConf.Source = "eyefill.svg";
        }
    }
    
}
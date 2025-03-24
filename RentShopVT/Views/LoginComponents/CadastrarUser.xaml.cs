using RentShopVT.ViewModels;

namespace RentShopVT.Views.LoginComponents;

public partial class CadastrarUser : ContentPage
{
	public CadastrarUser()
	{
		InitializeComponent();
        BindingContext = new CadastrarUserViewModel(Navigation);
    }

    async private void VoltarLoginCadastro_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }


    /*-------------------------------------------------------------------------Vizualizar e Esconder Senha--------------------------------------------------------------*/
    private void VizualizarConf_Clicked(object sender, EventArgs e)
    {
        ConfirmarSenha.IsPassword = !ConfirmarSenha.IsPassword;

        if (ConfirmarSenha.IsPassword)
        {
            VizualizarConf.Source = "eyeslashfill.svg";
        }
        else
        {
            VizualizarConf.Source = "eyefill.svg";
        }
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
    /*--------------------------------------------------------------------------------Fim e Esconder Senha--------------------------------------------------------------*/
}

using CommunityToolkit.Maui.Views;

namespace RentShopVT.Views.Components;

public partial class CaixaDeAlerta : Popup
{
	public CaixaDeAlerta(string TipoMensagem, string message, string cor)
	{
		InitializeComponent();
        MessageLabel.Text = TipoMensagem;
        MessageLabel.TextColor = Color.Parse(cor);
        MessageLabel2.Text = message; 
    }

    private void FecharPopup_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}
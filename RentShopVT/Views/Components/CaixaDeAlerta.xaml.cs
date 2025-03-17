using CommunityToolkit.Maui.Views;

namespace RentShopVT.Views.Components;

public partial class CaixaDeAlerta : Popup
{
	public CaixaDeAlerta(string message)
	{
		InitializeComponent();
        MessageLabel.Text = message;
    }

    private void FecharPopup_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}
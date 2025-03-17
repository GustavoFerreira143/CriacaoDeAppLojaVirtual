
namespace RentShopVT.Views.Components.HomePage;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}

    async private void Button_Clicked(object sender, EventArgs e)
    {

        var novaPagina = new TelaLogin();

        novaPagina.TranslationX= 300;

        await Navigation.PushModalAsync(novaPagina,false);
        await Task.WhenAll(
            this.TranslateTo(-300, 0, 180, Easing.Linear), // Move a página atual para a esquerda
            novaPagina.TranslateTo(0, 0, 180, Easing.Linear) // Move a nova página para o centro
        );
        await this.TranslateTo(0, 0, 180, Easing.Linear);
    }

}
public class CarrousselItem
{
    public string? ImageSource { get; set; }
    public string? Description { get; set; }
    public string? Preco { get; set; }
}
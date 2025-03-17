using RentShopVT.Views;
using RentShopVT.Views.Components;

namespace RentShopVT
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPageNovo();
        }
    }
}

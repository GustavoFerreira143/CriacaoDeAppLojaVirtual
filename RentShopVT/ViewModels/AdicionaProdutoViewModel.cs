using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentShopVT.Models;
using RentShopVT.Views.Components;
using System.Windows.Input;

namespace RentShopVT.ViewModels
{
    public partial class AdicionaProdutoViewModel : ObservableObject
    {
        public ICommand AdicionaQuantidade { get; }
        public ICommand ReduzQuantidade { get; }

        [ObservableProperty]
        private int quantidade;

        public AdicionaProdutoViewModel()
        {
            Quantidade = 0;
            AdicionaQuantidade = new RelayCommand(async () => await AdicionaValor());
            ReduzQuantidade = new RelayCommand(async () => await ReduzValor());
        }
        public async Task AdicionaValor()
        {
            Quantidade = Quantidade + 1;
        }
        public async Task ReduzValor()
        {
            if(Quantidade > 0)
            {
                Quantidade = Quantidade - 1;
            }
        }
    }
}

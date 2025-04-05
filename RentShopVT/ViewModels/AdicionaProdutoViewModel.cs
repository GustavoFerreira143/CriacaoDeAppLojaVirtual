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
        public ICommand AdicionarFoto { get; }


        [ObservableProperty]
        private int quantidade;

        public AdicionaProdutoViewModel()
        {
            Quantidade = 0;
            AdicionaQuantidade = new RelayCommand(async () => await AdicionaValor());
            ReduzQuantidade = new RelayCommand(async () => await ReduzValor());
            AdicionarFoto = new RelayCommand(async () => await AdicionarFotos());
        }
        public async Task AdicionaValor()
        {
            Quantidade = Quantidade + 1;
        }
        public async Task ReduzValor()
        {
            if (Quantidade > 0)
            {
                Quantidade = Quantidade - 1;
            }
        }
        public async Task AdicionarFotos()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecione uma imagem",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Erro", "Falha ao Adicionar Foto", "Red"));
            }

        }
    }
}

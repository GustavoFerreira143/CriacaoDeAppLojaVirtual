using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RentShopVT.Models;
using RentShopVT.Views.Components;

namespace RentShopVT.ViewModels
{
    public partial class RecuperaSenhaViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email;

        public ICommand EnviarCommand { get; }

        public RecuperaSenhaViewModel()
        {
            EnviarCommand = new RelayCommand(async () => await Enviar());
        }

        private async Task Enviar()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                Console.WriteLine("Erro: O campo de e-mail está vazio.");
                return;
            }

            try
            {
                // Chama o serviço de e-mail para enviar o token
                var response = await EmailService.Send(Email);

                if (response.IsSuccessful)
                {
                    Console.WriteLine("Chegou");
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("E-mail enviado com sucesso!"));

                }
                else
                {
                    Console.WriteLine($"Erro ao enviar e-mail: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }
    }
}

using Azure;
using CommunityToolkit.Maui.Views;
using RentShopVT.Models;
using RentShopVT.Views.Components;
using Mopups.Services;

namespace RentShopVT.ViewModels
{
    class EnvioDeImagensViewModel
    {
        public EnvioDeImagensViewModel()
        {

        }
        //----------------------------------------------------------------------------------------------------------Envio de Foto de Perfil--------------------------------------------------------------

        public async Task<string> SelecionarImagemAsync()
        {

            var foto = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecione uma imagem",
                FileTypes = FilePickerFileType.Images
            });


            EnvioDeImagensModel Enviar = new EnvioDeImagensModel();

            Retorno resposta = await Enviar.EnviarImagemAsync(foto);

            if (resposta.Status == "Sucesso")
            {

                Preferences.Set("FotoPerfil", "http://192.168.100.63:5098"+resposta.Link);
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", "Imagem Alterada Com Sucesso", "Green"));
                return resposta.Status;
            }
            else
            {
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", resposta.Link, "Red"));
                return resposta.Status;
            }
        }
    }
}

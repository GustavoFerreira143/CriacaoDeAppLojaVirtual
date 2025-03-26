using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentShopVT.ViewModels
{
    class EnvioDeImagensViewModel
    {

 //----------------------------------------------------------------------------------------------------------Envio de Foto de Perfil--------------------------------------------------------------

        public async Task<FileResult> SelecionarImagemAsync()
        {
            var foto = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecione uma imagem",
                FileTypes = FilePickerFileType.Images
            });

            return foto;
        }
    }
}

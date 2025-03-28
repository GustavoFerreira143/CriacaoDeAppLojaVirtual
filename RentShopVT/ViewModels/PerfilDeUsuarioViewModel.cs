using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using RentShopVT.Views.Components.PerfilUser.ModificarRedesDeUser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RentShopVT.ViewModels
{
    public partial class PerfilDeUsuarioViewModel : ObservableObject
    {
        //-----------------------------------------------------------------------Valores a Serem Modificados---------------------------------------------------------------------------
        public ICommand AbrirLink { get; }

        //                    "Facebook", "Instagram", "Linkedin", "Twitter", "Tiktok", "Youtube", "GitHub", "WhatsApp"

        public PerfilDeUsuarioViewModel()
        {

            AbrirLink = new RelayCommand(async () => await AbreLink());
        }

        public async Task AbreLink()
        {

            Launcher.OpenAsync("https://www.linkedin.com/in/gustavo-ferreira-238348231/");

        }
    }
}

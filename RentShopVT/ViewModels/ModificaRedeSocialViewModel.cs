using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using RentShopVT.Views.Components.PerfilUser.ModificarRedesDeUser;
using System.Windows.Input;
using RentShopVT.Views.Components;

namespace RentShopVT.ViewModels
{
    public partial class ModificaRedeSocialViewModel : ObservableObject
    {
        public ICommand EnviarRedesAtualizadas { get; }
        public ICommand ExibirSelecaoRedeSocial { get; }
        public ICommand FecharPopups { get; }
        //--------------------------------------------------------------Controle de Visibilidade das Redes Sociais para Lógica-----------------------------------------------

        // Propriedades das redes sociais
        [ObservableProperty] private bool linkedin;
        [ObservableProperty] private bool gitHub;
        [ObservableProperty] private bool facebook;
        [ObservableProperty] private bool instagram;
        [ObservableProperty] private bool twitter;
        [ObservableProperty] private bool whatsApp;
        [ObservableProperty] private bool tikTok;
        [ObservableProperty] private bool youtube;


        [ObservableProperty] private string linkedinText;
        [ObservableProperty] private string gitHubText;
        [ObservableProperty] private string facebookText;
        [ObservableProperty] private string instagramText;
        [ObservableProperty] private string twitterText;
        [ObservableProperty] private string whatsAppText;
        [ObservableProperty] private string tikTokText;
        [ObservableProperty] private string youtubeText;

        //------------------------------------------------------------------------------Redes Sociais que podem Ser Selecionadas------------------------------------------------------------

        [ObservableProperty] private bool linkedinUso;
        [ObservableProperty] private bool gitHubUso;
        [ObservableProperty] private bool facebookUso;
        [ObservableProperty] private bool instagramUso;
        [ObservableProperty] private bool twitterUso;
        [ObservableProperty] private bool whatsAppUso;
        [ObservableProperty] private bool tikTokUso;
        [ObservableProperty] private bool youtubeUso;

        private int Contagem { get; set; }
        private int RedesSelecionadas { get; set; }
        public ModificaRedeSocialViewModel()
        {
//--------------------------------------------------------------------------------------------Textos das Redes Sociais Manipulaveis---------------------------------------------------------
            LinkedinText = "https://www.linkedin.com/in/";
            GitHubText = "https://github.com/";
            FacebookText = "https://www.facebook.com/";
            InstagramText = "https://www.instagram.com/";
            TwitterText = "https://x.com/";
            WhatsAppText = "https://wa.me/";
            TikTokText = "https://www.tiktok.com/";
            YoutubeText = "https://www.youtube.com/";

            //--------------------------------------------------------------------------------Atualização dos dados em ModificaRedes---------------------------------------------------------
            Linkedin = Preferences.Get("Linkedin", false);
            GitHub = Preferences.Get("GitHub", false);
            Facebook = Preferences.Get("Facebook", false);
            Instagram = Preferences.Get("Instagram", false);
            Twitter = Preferences.Get("Twitter", false);
            WhatsApp = Preferences.Get("WhatsApp", false);
            TikTok = Preferences.Get("Tiktok", false);
            Youtube = Preferences.Get("Youtube", false);

            //--------------------------------------------------------------------------------Atualização dos dados em PopupSelecionarRede--------------------------------------------------
            LinkedinUso = Preferences.Get("Linkedin", false);
            GitHubUso = Preferences.Get("GitHub", false);
            FacebookUso = Preferences.Get("Facebook", false);
            InstagramUso = Preferences.Get("Instagram", false);
            TwitterUso = Preferences.Get("Twitter", false);
            WhatsAppUso = Preferences.Get("WhatsApp", false);
            TikTokUso = Preferences.Get("Tiktok", false);
            YoutubeUso = Preferences.Get("Youtube", false);

            ExibirSelecaoRedeSocial = new RelayCommand(async () => await AbrePopUp());
            FecharPopups = new RelayCommand(async () => await FecharPopup());
            EnviarRedesAtualizadas = new RelayCommand(async () => await AtualizaRedesSociaisSelecionadas());
        }

        public async Task AtualizaRedesSociaisSelecionadas()
        {
            List<string> RedesEscolhidas = new List<string>();
            RedesSelecionadas = 0;

            if(Linkedin)
            {
                RedesSelecionadas++;
                RedesEscolhidas.Add("Linkedin");
            }
            if (GitHub)
            {
                RedesSelecionadas++;
                RedesEscolhidas.Add("GitHub");
            }
            if (Facebook)
            {
                RedesSelecionadas++;
                RedesEscolhidas.Add("Facebook");
            }
            if (Instagram)
            {
                RedesSelecionadas++;
                RedesEscolhidas.Add("Instagram");
            }
            if (Twitter)
            {
                if(RedesSelecionadas < 4)
                {
                    RedesSelecionadas++;
                    RedesEscolhidas.Add("Twitter");
                }
                else
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Limite de Rede Sociais Atingido", "Red"));
                    return;
                }
            }
            if (WhatsApp)
            {
                if (RedesSelecionadas < 4)
                {
                    RedesSelecionadas++;
                    RedesEscolhidas.Add("WhatsApp");
                }
                else
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Limite de Rede Sociais Atingido", "Red"));
                    return;
                }
            }
            if(TikTok)
            {
                if (RedesSelecionadas < 4)
                {
                    RedesSelecionadas++;
                    RedesEscolhidas.Add("TikTok");
                }
                else
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Limite de Rede Sociais Atingido", "Red"));
                    return;
                }
            }
            if (Youtube)
            {
                if (RedesSelecionadas < 4)
                {
                    RedesSelecionadas++;
                    RedesEscolhidas.Add("Youtube");
                }
                else
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Limite de Rede Sociais Atingido", "Red"));
                    return;
                }
            }
        }


        //-------------------------------------------------------------------------------------Abre e Fecha PopUp de Seleção de redes Sociais---------------------------------------------------------
        public async Task AbrePopUp()
        {

            var popup = new PopupSelecionarRede();
            popup.BindingContext = this;
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                MopupService.Instance.PushAsync(popup);
            });
        }

        public async Task FecharPopup()
        {
            AtualizarRedesSociais();
            await MopupService.Instance.PopAsync();
        }
//------------------------------------------------------------------------------------Atualizar Redes Sociais ao fechamento do popUp----------------------------------------------------------
        public void AtualizarRedesSociais()
        {
            Linkedin = Preferences.Get("Linkedin", false);
            GitHub = Preferences.Get("GitHub", false);
            Facebook = Preferences.Get("Facebook", false);
            Instagram = Preferences.Get("Instagram", false);
            Twitter = Preferences.Get("Twitter", false);
            WhatsApp = Preferences.Get("WhatsApp", false);
            TikTok = Preferences.Get("Tiktok", false);
            Youtube = Preferences.Get("Youtube", false);
        }

        //------------------------------------------------------------------------------------Detecta mudanças nos CheckBox do popup----------------------------------------------------
        partial void OnLinkedinUsoChanged(bool value)
        {
            bool linkedin = AtualizarContagem(ref linkedinUso, value);
            Preferences.Set("Linkedin", linkedin);
            Console.WriteLine(linkedin ? "Linkedin Adicionado" : "Linkedin Retirado");
        }

        partial void OnGitHubUsoChanged(bool value)
        {
            bool github = AtualizarContagem(ref gitHubUso, value);
            Preferences.Set("GitHub", github);
            Console.WriteLine(github ? "GitHub Adicionado" : "GitHub Retirado");
        }

        partial void OnFacebookUsoChanged(bool value)
        {
            bool facebook = AtualizarContagem(ref facebookUso, value);
            Preferences.Set("Facebook", facebook);
            Console.WriteLine(facebook ? "Facebook Adicionado" : "Facebook Retirado");
        }

        partial void OnInstagramUsoChanged(bool value)
        {
            bool instagram = AtualizarContagem(ref instagramUso, value);
            Preferences.Set("Instagram", instagram);
            Console.WriteLine(instagram ? "Instagram Adicionado" : "Instagram Retirado");
        }

        partial void OnTwitterUsoChanged(bool value)
        {
            bool twitter = AtualizarContagem(ref twitterUso, value);
            Preferences.Set("Twitter", twitter);
            Console.WriteLine(twitter ? "Twitter Adicionado" : "Twitter Retirado");
        }

        partial void OnWhatsAppUsoChanged(bool value)
        {
            bool whatsapp = AtualizarContagem(ref whatsAppUso, value);
            Preferences.Set("WhatsApp", whatsapp);
            Console.WriteLine(whatsapp ? "WhatsApp Adicionado" : "WhatsApp Retirado");
        }

        partial void OnTikTokUsoChanged(bool value)
        {
            bool tiktok = AtualizarContagem(ref tikTokUso, value);
            Preferences.Set("Tiktok", tiktok);
            Console.WriteLine(tiktok ? "TikTok Adicionado" : "TikTok Retirado");
        }

        partial void OnYoutubeUsoChanged(bool value)
        {
            bool youtube = AtualizarContagem(ref youtubeUso, value);
            Preferences.Set("Youtube", youtube);
            Console.WriteLine(youtube ? "Youtube Adicionado" : "Youtube Retirado");
        }

        // ------------------------------------------------------------Verifica se Já não foi exedido total de redes selecionadas-----------------------------------------------------------------
        private bool AtualizarContagem(ref bool propriedade, bool value)
        {
            if (value)
            {
                if (Contagem < 4)
                {
                    propriedade = true;
                    Contagem++;
                    return true;
                }
                else
                {
                    Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Limite de Rede Sociais Atingido", "Red"));
                    propriedade = false; 
                    OnPropertyChanged(nameof(propriedade));
                    return false;
                }
            }
            else
            {
                propriedade = false;
                Contagem--;
                return false;
            }
        }
    }
}
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using RentShopVT.Views.Components.PerfilUser.ModificarRedesDeUser;
using System.Windows.Input;
using RentShopVT.Views.Components;
using System.Text.RegularExpressions;
using System.Text.Json;
using RentShopVT.Models;

namespace RentShopVT.ViewModels
{
    public partial class ModificaRedeSocialViewModel : ObservableObject
    {
        public INavigation _navigation;
        public ICommand EnviarRedesAtualizadas { get; }
        public ICommand ExibirSelecaoRedeSocial { get; }
        public ICommand FecharPopups { get; }
        //--------------------------------------------------------------------Controle de Visibilidade das Redes Sociais para Lógica--------------------------------------------------------

        // Propriedades das redes sociais
        [ObservableProperty] private bool linkedin;
        [ObservableProperty] private bool gitHub;
        [ObservableProperty] private bool facebook;
        [ObservableProperty] private bool instagram;
        [ObservableProperty] private bool twitter;
        [ObservableProperty] private bool whatsApp;
        [ObservableProperty] private bool tikTok;
        [ObservableProperty] private bool youtube;

        //----------------------------------------------------------------------------Textos fixos dos Itens---------------------------------------------------------------------------------
        [ObservableProperty] private string linkedinText;
        [ObservableProperty] private string gitHubText;
        [ObservableProperty] private string facebookText;
        [ObservableProperty] private string instagramText;
        [ObservableProperty] private string twitterText;
        [ObservableProperty] private string whatsAppText;
        [ObservableProperty] private string tikTokText;
        [ObservableProperty] private string youtubeText;
        //-------------------------------------------------------------------------------------------Entrys Com os Links----------------------------------------------------------------------------
        [ObservableProperty] private string valorWhatsApp;
        [ObservableProperty] private string valorTikTok;
        [ObservableProperty] private string valorYoutube;
        [ObservableProperty] private string valorTwitter;
        [ObservableProperty] private string valorFacebook;
        [ObservableProperty] private string valorGitHub;
        [ObservableProperty] private string valorLinkedin;
        [ObservableProperty] private string valorInstagram;

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
        public ModificaRedeSocialViewModel(INavigation navigation)
        {
            //--------------------------------------------------------------------------------------------Textos das Redes Sociais Manipulaveis---------------------------------------------------------
            _navigation = navigation;

            LinkedinText = "https://www.linkedin.com/in/";
            GitHubText = "https://github.com/";
            FacebookText = "https://www.facebook.com/";
            InstagramText = "https://www.instagram.com/";
            TwitterText = "https://x.com/";
            WhatsAppText = "https://wa.me/";
            TikTokText = "https://www.tiktok.com/";
            YoutubeText = "https://www.youtube.com/";

            /* ----------------------------------------------------------------------------Atualização dos dados em ModificaRedes Antigo---------------------------------------------------------
              Linkedin = Preferences.Get("Linkedin", false);
              GitHub = Preferences.Get("GitHub", false);
              Facebook = Preferences.Get("Facebook", false);
              Instagram = Preferences.Get("Instagram", false);
              Twitter = Preferences.Get("Twitter", false);
              WhatsApp = Preferences.Get("WhatsApp", false);
              TikTok = Preferences.Get("Tiktok", false);
              Youtube = Preferences.Get("Youtube", false);
            */
            /* --------------------------------------------------------------------------Atualização dos dados em PopupSelecionarRede Antigo--------------------------------------------------
             LinkedinUso = Preferences.Get("Linkedin", false);
             GitHubUso = Preferences.Get("GitHub", false);
             FacebookUso = Preferences.Get("Facebook", false);
             InstagramUso = Preferences.Get("Instagram", false);
             TwitterUso = Preferences.Get("Twitter", false);
             WhatsAppUso = Preferences.Get("WhatsApp", false);
             TikTokUso = Preferences.Get("Tiktok", false);
             YoutubeUso = Preferences.Get("Youtube", false);
            */
            //--------------------------------------------------------------------------------Atualização dos dados em ModificaRedes---------------------------------------------------------
            Linkedin = false;
            GitHub = false;
            Facebook = false;
            Instagram = false;
            Twitter = false;
            WhatsApp = false;
            TikTok = false;
            Youtube = false;

            //--------------------------------------------------------------------------------Atualização dos dados em PopupSelecionarRede--------------------------------------------------
            LinkedinUso = false;
            GitHubUso = false;
            FacebookUso = false;
            InstagramUso = false;
            TwitterUso = false;
            WhatsAppUso = false;
            TikTokUso = false;
            YoutubeUso = false;

            string json = Preferences.Get("RedesSociais", "");

            if (json != "" && json != "{}")
            {
                var objeto = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);

                if (objeto != null)
                {
                    foreach (var itens in objeto)
                    {
                        // Preencher o valor com base na chave
                        switch (itens.Key)
                        {
                            case "Linkedin":
                                ValorLinkedin = itens.Value[2];
                                Linkedin = true;
                                LinkedinUso = true;
                                break;
                            case "GitHub":
                                ValorGitHub = itens.Value[2];
                                GitHub = true;
                                GitHubUso = true;
                                break;
                            case "Facebook":
                                ValorFacebook = itens.Value[2];
                                Facebook = true;
                                FacebookUso = true;
                                break;
                            case "Instagram":
                                ValorInstagram = itens.Value[2];
                                Instagram = true;
                                InstagramUso = true;
                                break;
                            case "Twitter":
                                ValorTwitter = itens.Value[2];
                                Twitter = true;
                                TwitterUso = true;
                                break;
                            case "WhatsApp":
                                ValorWhatsApp = itens.Value[2];
                                WhatsApp = true;
                                WhatsAppUso = true;
                                break;
                            case "TikTok":
                                ValorTikTok = itens.Value[2];
                                TikTok = true;
                                TikTokUso = true;
                                break;
                            case "Youtube":
                                ValorYoutube = itens.Value[2];
                                Youtube = true;
                                YoutubeUso = true;
                                break;
                        }
                    }
                }
            }
            ExibirSelecaoRedeSocial = new RelayCommand(async () => await AbrePopUp());
            FecharPopups = new RelayCommand(async () => await FecharPopup());
            EnviarRedesAtualizadas = new RelayCommand(async () => await AtualizaRedesSociaisSelecionadas());
        }
//----------------------------------------------------------------------------------------------------Atualiza Rede Sociais--------------------------------------------------------------------------------------------
        public async Task AtualizaRedesSociaisSelecionadas()
        {
            var redesEscolhidas = new Dictionary<string, List<string>>();
            int redesSelecionadas = 0;

            // Lista de redes sociais com os textos fixos e valores correspondentes
            var redes = new List<(bool Selecionado,string Nome, string imagem, string Texto, string Valor)>
            {
                (Linkedin,"Linkedin", "linkedin.svg", LinkedinText, ValorLinkedin),
                (GitHub,"GitHub", "github.svg", GitHubText, ValorGitHub),
                (Facebook,"Facebook", "facebook.svg", FacebookText, ValorFacebook),
                (Instagram,"Instagram", "instagram.svg", InstagramText, ValorInstagram),
                (Twitter,"Twitter","twitterx.svg", TwitterText, ValorTwitter),
                (WhatsApp,"WhatsApp", "whatsapp.svg", WhatsAppText, ValorWhatsApp),
                (TikTok,"TikTok", "tiktok.svg", TikTokText, ValorTikTok),
                (Youtube,"Youtube", "youtube.svg", YoutubeText, ValorYoutube)
            };

            foreach (var rede in redes)
            {
                if (rede.Selecionado)
                {
                    if (redesSelecionadas >= 4)
                    {
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", "Limite de Rede Sociais Atingido, Retire Alguma", "Red"));
                        return ;
                    }

                    if (string.IsNullOrWhiteSpace(rede.Valor))
                    {
                        Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", $"O campo '{rede.Nome}' está vazio. Insira um link.", "Red"));
                        return ;
                    }
                    var valores = new List<string>();
                    string conteudo = SanitizarNomeUsuario(rede.Valor);

                    valores.Add(rede.imagem);
                    valores.Add(rede.Texto);
                    valores.Add(rede.Valor);

                    redesEscolhidas.Add(rede.Nome, valores);
                    redesSelecionadas++;
                }
            }

            var popup = new TelaLoading();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                MopupService.Instance.PushAsync(popup);
            });

            ModificaRedeSocialModel modificaRedeSocialModel = new ModificaRedeSocialModel();
            var resposta = await modificaRedeSocialModel.ModificaRedeSocial(redesEscolhidas);

            await Task.Delay(2000);

            if(resposta == null)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (MopupService.Instance.PopupStack.Contains(popup))
                        MopupService.Instance.PopAsync();
                });
                Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("ERRO", $"Houve erro no momento de Envio tente Novamente", "Red"));
                return;
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (MopupService.Instance.PopupStack.Contains(popup))
                    MopupService.Instance.PopAsync();
            });

            string json = JsonSerializer.Serialize(redesEscolhidas);
            Preferences.Set("RedesSociais", json);

            Application.Current.MainPage.ShowPopup(new CaixaDeAlerta("Sucesso", $"Redes Sociais Salvas Com Sucesso", "Green"));
            _navigation.PopModalAsync();
            return ;
        }
        public string SanitizarNomeUsuario(string nomeUsuario)
        {
            return Regex.Replace(nomeUsuario, @"[^a-zA-Z0-9_.-]", "");
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
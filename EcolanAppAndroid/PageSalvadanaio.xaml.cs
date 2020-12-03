using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcolanAppAndroid {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSalvadanaio : ContentPage {
        public void SetHandlerMenu() {
            Menu1.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new HomePage()); };
            Menu2.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageMappa()); };
            //Menu3.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageSalvadanaio()); };
            Menu4.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageRaccolta()); };
            Menu5.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageAccesso()); };

        }
        public PageSalvadanaio() {
            InitializeComponent();
            SetHandlerMenu();
        }
        protected override void OnAppearing() {
            base.OnAppearing();
            var Qr = Preferences.Get("QRCODE", "");
            try {
                var soap = new SoapRequest("http://ecocontrolgsm.cloud/webservice/MonetaVirtuale/MonetaVirtuale.asmx", "AppRaccolta");
                soap.Parametri.Add("QrCode", Qr);
                var resp = soap.GetResponse();
                var respString = resp.ResponseString;
                var respBody = resp.XmlOuterBody;
                var ecopunti=resp.RispostaItem("Ecopunti");
                var CountPet = resp.RispostaItem("CountPet");
                var KgOlio = Convert.ToDecimal(resp.RispostaItem("KgOlio").ToString().Replace(".",","));
                var MonetaVirtuale = Convert.ToDecimal(resp.RispostaItem("MonetaVirtuale").ToString());
                LabelRaccolto.Text = "FIN ORA HAI RACCOLTO: " + CountPet.ToString() + "pz di PET e " + decimal.Round(KgOlio,0).ToString() + "Kg di Olio.";
                LabelLtAcqua.Text = "Hai diritto a prendere " + decimal.Round((MonetaVirtuale/5),0).ToString() + " Lt di acqua potabile filtrata e refrigerata.";
                LabelMlDetergente.Text = "Hai diritto a prendere " + decimal.Round((MonetaVirtuale/18), 0).ToString() + " ml di detergente ecologico.";
            } catch (Exception e) {
                DisplayAlert("", e.Message, "OK");
            }
        }
    }
}
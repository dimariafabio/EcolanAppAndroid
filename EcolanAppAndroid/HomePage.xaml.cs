using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcolanAppAndroid {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage {
        private String Qr;
        public void SetHandlerMenu() {
            //Menu1.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new HomePage()); };
            Menu2.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageMappa()); };
            Menu3.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageSalvadanaio()); };
            Menu4.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageRaccolta()); };
            Menu5.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageAccesso()); };

        }
        public HomePage() {
            InitializeComponent();
            SetHandlerMenu();
            //TEST ApiParco
            var Ap = new ClassApiParco();
            var table=Ap.EseguiQuery("Select * From Utente");

            //

            //Qr Code
            Qr = Preferences.Get("QRCODE", "");
            if (Qr == "") {
                BarCodeId.IsVisible = false;
                try {
                    var soap = new SoapRequest("http://ecocontrolgsm.cloud/webservice/monetavirtuale/monetavirtuale.asmx", "CreaQRCodeMonetaVirtuale");
                    soap.Parametri.Add("Paese", "ECOPARCO " + Xamarin.Essentials.DeviceInfo.Model + " " + Xamarin.Essentials.DeviceInfo.Name);
                    soap.Parametri.Add("IdentificativoBidone", "");
                    soap.Parametri.Add("ImportoInCentesimi", 0);
                    soap.Parametri.Add("CodiceFiscale", "");
                    var resp = soap.GetResponse();
                    Qr = resp.RispostaItem("QrCode").ToString();
                    //LabelQrCode.Text = Qr;
                    Preferences.Set("QRCODE", Qr);
                    BarCodeId.BarcodeValue = Qr;
                    BarCodeId.IsVisible = true;
                    DisplayAlert("", "Ho generato un nuovo QRCODE. Puoi ora utilizzarlo!", "OK");
                } catch (Exception e) {
                    DisplayAlert("", e.Message, "cancel");
                    LblErrore.IsVisible = true;
                    //LabelQrCode.Text = "";
                }
            } else {
                BarCodeId.BarcodeValue = Qr;
                //LabelQrCode.Text = Qr;
                BarCodeId.IsVisible = true;
            }
            AggiornaCredito();
            FCM.FcmSetTopics();
            Device.StartTimer(TimeSpan.FromSeconds(6), ()=>{
                AggiornaCredito();
                return true;
            });
        }
        private void AggiornaCredito() {
            LabelCredito.FadeTo(0, 250);
            try {
                var soap = new SoapRequest("http://ecocontrolgsm.cloud/webservice/monetavirtuale/monetavirtuale.asmx", "MonetaVirtualeVisualizzaCredito");
                soap.Parametri.Add("QrCode", Qr);
                var resp = soap.GetResponse();
                Device.BeginInvokeOnMainThread(() => {
                    LabelCredito.Text = String.Format("{0:0.00}", decimal.Parse(resp.ResponseString) / 100);
                    LabelCredito.FadeTo(1, 500);
                });
            } catch (Exception) {
                Device.BeginInvokeOnMainThread(() => {
                    LabelCredito.Text = "N.D.";
                    LabelCredito.FadeTo(1, 500);
                });
            }

        }
        private void BtnAggiorna_Clicked(object sender, EventArgs e) {
            AggiornaCredito();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace EcolanAppAndroid {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRaccolta : ContentPage {
        public void SetHandlerMenu() {
            Menu1.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new HomePage()); };
            Menu2.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageMappa()); };
            Menu3.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageSalvadanaio()); };
            //Menu4.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageRaccolta()); };
            Menu5.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageAccesso()); };

        }
        public PageRaccolta() {
            InitializeComponent();
            SetHandlerMenu();
        }

        private async void BtnScan_Clicked(object sender, EventArgs e) {
            
            var options = new MobileBarcodeScanningOptions {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true
            };
            var overlay = new ZXingDefaultOverlay {
                TopText = "SCANSIONA BARCODE DEL PRODOTTO",
                BottomText = ""
            };
            var Pagescanner = new ZXingScannerPage(options,overlay);
            await Navigation.PushAsync(Pagescanner);
            Pagescanner.OnScanResult += (x) => {
                Pagescanner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async() => {
                    await Navigation.PopAsync();
                    var soap = new SoapRequest("http://ecocontrolgsm.cloud/webservice/WebServiceAndroid.asmx", "DizionarioRifiutoGet");
                    soap.namespac = "http://tempuri.org/";
                    soap.AddBody("QrCode", x.Text);
                    try {
                        var resp = soap.GetResponse();
                        var TipoRifiuto = resp.ResponseString;
                        if (TipoRifiuto == "") TipoRifiuto = "Sconosciuto! Puoi comunque aggiungerlo."; else TipoRifiuto = "Rifiuto riconosciuto: " + TipoRifiuto;
                        await DisplayAlert("", TipoRifiuto, "OK");
                    }catch(Exception e) {
                        await DisplayAlert("Errore", "Connessione internet non disponibile!", "OK");
                    }
                });
            };
        }

        private async void BtnScanNuovoProdotto_Clicked(object sender, EventArgs e) {
            var options = new MobileBarcodeScanningOptions {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true
            };
            var overlay = new ZXingDefaultOverlay {
                TopText = "SCANSIONA BARCODE DEL PRODOTTO",
                BottomText = ""
            };
            var Pagescanner = new ZXingScannerPage(options,overlay);
            await Navigation.PushAsync(Pagescanner);
            Pagescanner.OnScanResult += (x) => {
                Pagescanner.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () => {
                    await Navigation.PopAsync();
                    var QrLetto = x.Text;
                    if (QrLetto == "") return;
                    var Page = new PageRifiutiScelta(QrLetto);
                    await Navigation.PushAsync(Page);
                });
            };
        }

        private async void BtnScanSacco_Clicked(object sender, EventArgs e) {

            var options = new MobileBarcodeScanningOptions {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true
            };
            var overlay = new ZXingDefaultOverlay {
                TopText = "SCANSIONA BARCODE DEL PRODOTTO",
                BottomText = ""
            };
            var Pagescanner = new ZXingScannerPage(options, overlay);
            await Navigation.PushAsync(Pagescanner);
            Pagescanner.OnScanResult += (x) => {
                Pagescanner.IsScanning = false;
                Device.BeginInvokeOnMainThread(() => {
                    Navigation.PopAsync();
                    var QrLetto = x.Text;
                    if (QrLetto.Length <= 16) { DisplayAlert("", "Sacchetto non riconosciuto!", "OK"); }
                    var TipoRifiuto = "";
                    switch (QrLetto.Substring(0, 3)) {
                        case "CAR":
                            TipoRifiuto = "CARTA";
                            break;
                        case "PLA":
                            TipoRifiuto = "PLASTICA";
                            break;
                        case "VET":
                            TipoRifiuto = "VETRO";
                            break;
                        case "ORG":
                            TipoRifiuto = "ORGANICO";
                            break;
                        case "IND":
                            TipoRifiuto = "INDIFFERENZIATO";
                            break;
                    }
                    if (TipoRifiuto!="") DisplayAlert("", TipoRifiuto, "OK");
                });
            };
        }
    }
}
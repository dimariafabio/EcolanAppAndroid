using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.QrCode.Internal;

namespace EcolanAppAndroid {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRifiutiScelta : ContentPage {
        private string QRCode;
        public PageRifiutiScelta(String QrCode) {
            InitializeComponent();
            this.QRCode = QrCode;
        }
        public async void InviaRichiesta(String TipoRifiuto) {
            var soap = new SoapRequest("http://ecocontrolgsm.cloud/webservice/WebServiceAndroid.asmx", "DizionarioRifiutoAdd");
            soap.namespac = "http://tempuri.org/";
            soap.AddBody("QrCode", QRCode);
            soap.AddBody("TipoRifiuto", TipoRifiuto);
            try {
                var resp = soap.GetResponse();
                await DisplayAlert("", "Grazie per aver aggiunto il rifiuto!", "OK");
            }catch(Exception e) {
                await DisplayAlert("", "Connessione internet non disponibile!", "OK");
            }
            await Navigation.PopAsync();
        }

        private void BtnCarta_Clicked(object sender, EventArgs e) {
            InviaRichiesta("Carta");
        }

        private void BtnPlastica_Clicked(object sender, EventArgs e) {
            InviaRichiesta("Plastica");
        }

        private void BtnVetro_Clicked(object sender, EventArgs e) {
            InviaRichiesta("Vetro");
        }

        private void BtnMetalli_Clicked(object sender, EventArgs e) {
            InviaRichiesta("Metalli");
        }

        private void BtnSecco_Clicked(object sender, EventArgs e) {
            InviaRichiesta("Indifferenziato");
        }

        private void BtnUmido_Clicked(object sender, EventArgs e) {
            InviaRichiesta("Organico");
        }
    }
}
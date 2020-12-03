using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.QrCode.Internal;

namespace EcolanAppAndroid {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAccesso : ContentPage {
        public void SetHandlerMenu() {
            Menu1.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new HomePage()); };
            Menu2.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageMappa()); };
            Menu3.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageSalvadanaio()); };
            Menu4.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageRaccolta()); };
            //Menu5.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageAccesso()); };

        }
        public PageAccesso() {
            InitializeComponent();
            SetHandlerMenu();
            
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            TextCf.Text = Preferences.Get("CF", "");
        }
        protected override void OnDisappearing() {
            base.OnDisappearing();
            Preferences.Set("CF", TextCf.Text.ToUpper());
        }


        

        private void Entry_TextChanged(object sender, TextChangedEventArgs e) {
            if (TextCf.Text!="") BarCodeId.BarcodeValue = TextCf.Text.ToUpper();
        }
    }
}
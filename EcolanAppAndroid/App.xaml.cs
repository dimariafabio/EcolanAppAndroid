using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace EcolanAppAndroid {
    public partial class App : Application {
        public App() {
            InitializeComponent();
            MainPage = new NavigationPage(new HomePage());//HomePage
            FCM.FCMGestione();
            FCM.FcmSetTopics();
        }
        public static Page GetMainPage() {
            return new ContentPage {
                Content = new Xamarin.Forms.Maps.Map(MapSpan.FromCenterAndRadius(new Position(37, -122), Distance.FromMiles(10)))
            };
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }


    }
}

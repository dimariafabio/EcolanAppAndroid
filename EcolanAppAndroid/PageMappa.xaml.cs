using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace EcolanAppAndroid {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMappa : ContentPage {
        public void SetHandlerMenu() {
            Menu1.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new HomePage()); };
            //Menu2.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageMappa()); };
            Menu3.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageSalvadanaio()); };
            Menu4.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageRaccolta()); };
            Menu5.Clicked += async (s, e) => { await Application.Current.MainPage.Navigation.PushAsync(new PageAccesso()); };

        }

        public async void MappaXamarinAsync() {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 500;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
            map1.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMiles(1)));
            

        }
        
        public PageMappa() {
            InitializeComponent();
            SetHandlerMenu();
            Task.Run(() => MappaXamarinAsync());
        }

    }
}
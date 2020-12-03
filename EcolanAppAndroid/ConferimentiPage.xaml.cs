using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcolanAppAndroid {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConferimentiPage : ContentPage {
        public ConferimentiPage() {
            InitializeComponent();
        }

        private void BtnIndietro_Clicked(object sender, EventArgs e) {
            Navigation.PopAsync();
        }
    }
}
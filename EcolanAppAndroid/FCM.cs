using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcolanAppAndroid {
    class FCM {
        public static void FcmSetTopics() {
            var ListaTopics = new List<String>();
            var QrCode = Preferences.Get("QRCODE", "");
            if (QrCode != "") ListaTopics.Add(QrCode);
            CrossFirebasePushNotification.Current.UnsubscribeAll();
            CrossFirebasePushNotification.Current.Subscribe(ListaTopics.ToArray());

        }

        public static void FCMGestione() {
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) => {
                System.Diagnostics.Debug.WriteLine("Opened");
            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) => {
                System.Diagnostics.Debug.WriteLine("Received");
                Device.BeginInvokeOnMainThread(async () => {
                    await Application.Current.MainPage.DisplayAlert("", p.Data["body"].ToString(), "OK");
                });

            };

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) => {
                FcmSetTopics();
            };

        }
    }
}

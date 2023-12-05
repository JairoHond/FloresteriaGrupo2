using FloresteriaGrupo2.Views;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Rating;
using Plugin.FirebasePushNotification;

namespace FloresteriaGrupo2
{
         public partial class App : Application
        {
        public static string Data { get; set; }
        public App()
            {
                InitializeComponent();
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                Console.WriteLine($"TOKEN : {p.Token}");
                Console.WriteLine("Hola Mundo");
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Received");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key}:{data.Value}");
                }
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach(var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };
            }

            protected override void OnStart()
            {
                Iniciar();
            }

            protected override void OnSleep()
            {
            }

            protected override void OnResume()
            {
                Debug.WriteLine("OnResume");

            }

            public async void Iniciar()
            {
                if ((Preferences.Get("Remember", true) == true))
                {
                    MainPage = new PageLogin();
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(PageInicio)}");
                }
            }
        }
    }


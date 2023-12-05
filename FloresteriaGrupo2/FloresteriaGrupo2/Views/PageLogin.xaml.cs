using FloresteriaGrupo2.Modelo;
using FloresteriaGrupo2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloresteriaGrupo2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageLogin : ContentPage
    {
        ApiServiceAuthentication _userRepository = new ApiServiceAuthentication();

        public PageLogin()
        {
            InitializeComponent();
        }

        private async void BtnIniciarSesion_Clicked(object sender, EventArgs e)
        {
            if (txtContrasena.Text.Trim().Equals("") || txtEmail.Text.Trim().Equals(""))
            {
                await DisplayAlert("Oops", "Ingrese todos los datos", "Aceptar");
                return;
            }

            UserAuthentication oUsuario = new UserAuthentication()
            {
                email = txtEmail.Text,
                password = txtContrasena.Text,
                returnSecureToken = true
            };

            bool respuesta = await ApiServiceAuthentication.Login(oUsuario);
            if (respuesta)
            {

                Application.Current.MainPage = new PageInicio();
            }
            else
            {
                await DisplayAlert("Oops", "Usuario no encontrado", "OK");


            }
        }

        private void BtnRegistrarse_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PageRegistro());
        }

        private void ForgotTap_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Views.PageRestaurarContraseña());
        }
    }
}
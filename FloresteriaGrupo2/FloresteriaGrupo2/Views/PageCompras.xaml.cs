using FloresteriaGrupo2.Modelo;
using FloresteriaGrupo2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloresteriaGrupo2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCompras : ContentPage
    {
        public Compra Site;
        bool editando = false;

        public List<Compra> oListaCompra { get; set; }
        public PageCompras()
        {
            InitializeComponent();
            ObtenerCompra();
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (editando)
            {
                

                editando = false;

                Site = null;
            }
        }

        private async void ObtenerCompra()
        {
            List<Compra> oObjecto = await ApiServiceFirebase.ObtenerCompra();

            if (oObjecto != null)
            {
                oListaCompra = oObjecto;
                ListViewCompra.ItemsSource = oObjecto;
            }

        }

        //private async void btnenviar_Clicked(object sender, EventArgs e)
        //{

        //    await Navigation.PushModalAsync(new PageCalificar());

        //}

        private async void listSites_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                Site = e.Item as Compra;


            }
            catch (Exception ex)
            {
                Message("Error:", ex.Message);
            }

        }

        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }

        //private void btnenviarCali_Clicked(object sender, EventArgs e)
        //{
        //    int Rating = rating.SelectedStarValue;
        //    DisplayAlert("Gracias Por Calificarnos", Rating.ToString(), "Ok");
         
        //}

        //private void TapLabelTerminosCondiciones_Tapped(object sender, EventArgs e)
        //{
        //    popupTerminosCondiciones.IsVisible = true;
        //}

        //private void btnCerrarModal_Clicked(object sender, EventArgs e)
        //{
        //    popupTerminosCondiciones.IsVisible = false;
        //    IsEnabled = true;
        //}

        //private void TapLabelTerminosCondicionesCerrar_Tapped(object sender, EventArgs e)
        //{
        //    popupTerminosCondiciones.IsVisible = false;
        //    IsEnabled = true;
        //}
    }
}
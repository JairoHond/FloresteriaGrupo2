using FloresteriaGrupo2.Modelo;
using FloresteriaGrupo2.Service;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FloresteriaGrupo2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagePerfil : ContentPage
    {
        public Usuario Usuarios;

        byte[] Image;
        MediaFile FileFoto = null;
        public PagePerfil()
        {
            InitializeComponent();
            obtenerUsuario();
        }
        Usuario oGlobalUsuario;

        private async void BtnGuardarCambios_Clicked(object sender, EventArgs e)
        {
            if (Image == null)
            {
                Image = Image;
            }

            if (txtNombre.Text.Trim() == "" || txtApellido.Text.Trim() == "" || txtDocumento.Text.Trim() == "")
            {
                await DisplayAlert("Mensaje", "Debe completar todos los campos", "OK");
                return;
            }


            Usuario oUsuario = new Usuario()
            {
                Nombres = txtNombre.Text,
                Apellidos = txtApellido.Text,
                Documento = txtDocumento.Text,
                Image = Image,
                // Clave = txtPassword.Text,
                Clave = oGlobalUsuario.Clave,
                Email = oGlobalUsuario.Email

            };

            bool respuesta = await ApiServiceFirebase.GuardarCambiosUsuario(oUsuario);

            if (respuesta)
            {
                await DisplayAlert("Mensaje", "Se guardaron los cambios", "OK");
            }
            else
            {
                await DisplayAlert("Mensaje", "Hubo un error al guardar", "OK");
            }

        }

        private async void obtenerUsuario()
        {

            oGlobalUsuario = await ApiServiceFirebase.ObtenerUsuario();

            if (oGlobalUsuario != null)
            {
                Imagen.Source = GetImageResourseFromBytes(oGlobalUsuario.Image);
                txtNombre.Text = oGlobalUsuario.Nombres;
                txtApellido.Text = oGlobalUsuario.Apellidos;
              //  txtPassword.Text = oGlobalUsuario.Clave;
                txtDocumento.Text = oGlobalUsuario.Documento;
                txtEmail.Text = oGlobalUsuario.Email;

            }
        }

        private ImageSource GetImageResourseFromBytes(byte[] bytes)
        {
            ImageSource retSource = null;

            if (bytes != null)
            {
                byte[] imageAsBytes = (byte[])bytes;
                retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }

            return retSource;
        }

        private void BtnCerrarSesion_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new PageLogin();
        }

        private async void btnModificarFoto_Clicked(object sender, EventArgs e)
        {
            bool response = await Application.Current.MainPage.DisplayAlert("Advertencia", "Realizar la opción mendiante: ", "Camara", "Galeria");

            if (response)
                GetImageFromCamera();
            else
                GetImageFromGallery();


        }

        private async void GetImageFromCamera()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
            if (status == PermissionStatus.Granted)
            {
                try
                {
                    FileFoto = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                        SaveToAlbum = true
                    });

                    if (FileFoto == null)
                        return;

                    Imagen.Source = ImageSource.FromStream(() => { return FileFoto.GetStream(); });
                    Image = File.ReadAllBytes(FileFoto.Path);
                }
                catch (Exception)
                {
                    Message("Advertencia", "Se produjo un error al tomar la fotografia.");
                }
            }
            else
            {
                await Permissions.RequestAsync<Permissions.Camera>();
            }


        }

        private async void GetImageFromGallery()
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    var FileFoto = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                    });
                    if (FileFoto == null)
                        return;

                    Imagen.Source = ImageSource.FromStream(() => { return FileFoto.GetStream(); });
                    Image = File.ReadAllBytes(FileFoto.Path);
                }
                else
                {
                    Message("Advertencia", "Se produjo un error al seleccionar la imagen");
                }
            }
            catch (Exception)
            {
                Message("Advertencia", "Se produjo un error al seleccionar la imagen");
            }

        }
        private async void Message(string title, string message)
        {
            await DisplayAlert(title, message, "OK");
        }


        private Byte[] ConvertImageToByteArray()
        {
            if (FileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();

                    stream.CopyTo(memory);

                    return memory.ToArray();
                }
            }

            return null;
        }

        private async void VerUbicacionTap_Tapped(object sender, EventArgs e)
        {
            var status = await DisplayAlert("Aviso", $"¿Desea ir a la ubicacion indicada?", "SI", "NO");

            if (status)
            {
                await Navigation.PushModalAsync(new PageMapa(Usuarios));
            }

        }
    }
}
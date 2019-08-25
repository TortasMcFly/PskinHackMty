using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pskin.API;
using Pskin.Models;
using Pskin.Utils;
using Pskin.Views.Home;
using Xamarin.Forms;

namespace Pskin.Views.Login
{
    public partial class SubirFotoPage : ContentPage
    {
        byte[] imagen;
        Usuario user;
        public SubirFotoPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //Establecer la opacidad de la vista en cero
            StackContent.Opacity = 0;

            user = JsonConvert.DeserializeObject<Usuario>(Application.Current.Properties["Usuario"].ToString());
            lblBienvenida.Text = "Bienvenido " + user.Nombre;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (imagen == null)
            {
                //Iniciar animación
                await Task.WhenAll(
                   Animacion.Appear(StackContent)
                );
            }

        }

        async void Subir_Clicked(object sender, System.EventArgs e)
        {
            imagen = await Camara.TomarFoto();

            if (imagen != null)
            {
                var stream = new MemoryStream(imagen);
                ImageSource retSource = ImageSource.FromStream(() => stream);

                profileImage.Source = retSource;

                await BtnContinuar.FadeTo(1, 250, Easing.Linear);
            }
        }

        async void Galeria_Clicked(object sender, System.EventArgs e)
        {
            imagen = await Camara.ElegirFoto();

            if (imagen != null)
            {
                var stream = new MemoryStream(imagen);
                ImageSource retSource = ImageSource.FromStream(() => stream);

                profileImage.Source = retSource;

                await BtnContinuar.FadeTo(1, 250, Easing.Linear);
            }
        }

        async void Continuar_Clicked(object sender, System.EventArgs e)
        {
            if (await PskinAPI.SubirImagen(imagen, user.Id))
            {
                await Animacion.Disappear(GridContent);
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                await DisplayAlert("Ocurrió un error", "Vuelve a intentarlo", "Aceptar");
            }

            
        }

        //Saltar
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Animacion.Disappear(GridContent);
            Application.Current.MainPage = new NavigationPage(new HomePage());
        }

    }
}

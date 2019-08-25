using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Pskin.API;
using Pskin.Models;
using Pskin.Utils;
using Xamarin.Forms;

namespace Pskin.Views.Home
{
    public partial class NuevaHistoriaPage : ContentPage
    {
        bool abrirCamara = false;
        byte[] imagen;
        public NuevaHistoriaPage(bool abrirCamara)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //Volver
            var gestureVolver = new TapGestureRecognizer();
            gestureVolver.Tapped += GestureVolver_Tapped;
            lblVolver.GestureRecognizers.Add(gestureVolver);

            //Publicar
            var gesturePublicar = new TapGestureRecognizer();
            gesturePublicar.Tapped += GesturePublicar_Tapped;
            publicar.GestureRecognizers.Add(gesturePublicar);

            //Tomar foto
            var gestureTomar = new TapGestureRecognizer();
            gestureTomar.Tapped += GestureTomar_Tapped; 
            Camera.GestureRecognizers.Add(gestureTomar);

            //Agregar de la galeria
            var gestureGaleria = new TapGestureRecognizer();
            gestureGaleria.Tapped += GestureGaleria_Tapped; 
            Galeria.GestureRecognizers.Add(gestureGaleria);

            this.abrirCamara = abrirCamara;
        }


        protected async override void OnAppearing()
        {
            if(abrirCamara)
            {
                imagen = await Camara.TomarFoto();

                if (imagen != null)
                {
                    lblInfo.IsVisible = false;
                    var stream = new MemoryStream(imagen);
                    ImageSource retSource = ImageSource.FromStream(() => stream);

                    Imagen.Source = retSource;
                }
            }

            

            base.OnAppearing();
        }

        private async void GestureGaleria_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(Galeria);
            await Animacion.Deseleccionar(Galeria);

            imagen = await Camara.ElegirFoto();

            if (imagen != null)
            {
                lblInfo.IsVisible = false;
                var stream = new MemoryStream(imagen);
                ImageSource retSource = ImageSource.FromStream(() => stream);

                Imagen.Source = retSource;
            }
        }

        private async void GestureTomar_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(Camera);
            await Animacion.Deseleccionar(Camera);

            imagen = await Camara.TomarFoto();

            if (imagen != null)
            {
                lblInfo.IsVisible = false;
                var stream = new MemoryStream(imagen);
                ImageSource retSource = ImageSource.FromStream(() => stream);

                Imagen.Source = retSource;
            }
        }

        private async void GesturePublicar_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(publicar);
            await Animacion.Deseleccionar(publicar);
            if (imagen == null)
            {
                await DisplayAlert("Atención", "Agrega una imágen para publicar", "Aceptar");
                return;
            }

            if(string.IsNullOrEmpty(entry.Text))
            {
                await DisplayAlert("Atención", "Agrega una descripción para publicar", "Aceptar");
                return;
            }

            Usuario user = JsonConvert.DeserializeObject<Usuario>(Application.Current.Properties["Usuario"].ToString());

            if (await PskinAPI.SubirHistoria(imagen, user.Id, entry.Text))
            {
                await DisplayAlert("¡Bien hecho!", "Se publicó tu historia", "Aceptar");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Ocurrió un error", "Vuelve a intentarlo", "Aceptar");
            }

            
        }

        async void GestureVolver_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblVolver);
            await Animacion.Deseleccionar(lblVolver);

            await Navigation.PopModalAsync();
        }
    }
}

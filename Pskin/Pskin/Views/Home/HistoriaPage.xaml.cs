using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pskin.Models;
using Pskin.Utils;
using Xamarin.Forms;

namespace Pskin.Views.Home
{
    public partial class HistoriaPage : ContentPage
    {
        public HistoriaPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //Volver
            var gestureVolver = new TapGestureRecognizer();
            gestureVolver.Tapped += GestureVolver_Tapped;
            lblVolver.GestureRecognizers.Add(gestureVolver);

            Historia historia = JsonConvert.DeserializeObject<Historia>(Application.Current.Properties["Historia"].ToString());
            
            username.Text = historia.Usuario.Nombre + " " + historia.Usuario.Apellido;
            Imagen.Source = new UriImageSource
            {
                Uri = new Uri(historia.UrlImagen),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0)
            };

            //tieneLike = false;
            likes.Text = historia.Likes.ToString();
            timeAgo.Text = historia.TimeAgo;
            descripcion.Text = historia.Descripcion;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.WhenAll(
              //Animacion.Appear(GridContent),
              Imagen.ScaleTo(1, 300, Easing.Linear),
              Grid.TranslateTo(0, 0, 300, Easing.Linear),
              StackPost.TranslateTo(0, 0, 600, Easing.Linear)
           );


        }

        async void GestureVolver_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblVolver);
            await Animacion.Deseleccionar(lblVolver);

            await Task.WhenAll(
              Grid.TranslateTo(0, 100, 300, Easing.Linear)
           );

            await Navigation.PopAsync(false);
        }
    }
}

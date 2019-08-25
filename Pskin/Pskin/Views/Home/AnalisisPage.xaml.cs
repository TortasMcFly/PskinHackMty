using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pskin.Models;
using Pskin.Utils;
using Xamarin.Forms;

namespace Pskin.Views.Home
{
    public partial class AnalisisPage : ContentPage
    {
        public AnalisisPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //Volver
            var gestureVolver = new TapGestureRecognizer();
            gestureVolver.Tapped += GestureVolver_Tapped;
            lblVolver.GestureRecognizers.Add(gestureVolver);


            Analisis analisis = JsonConvert.DeserializeObject<Analisis>(Application.Current.Properties["Analisis"].ToString());

            timeAgo.Text = analisis.TimeAgo;
            Image.Source = analisis.UrlImagen;
            lblTag.Text = analisis.Tag;
            lblPorcentaje.Text = (analisis.Probabilidad * 100).ToString("N") + "%";

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.WhenAll(
              //Animacion.Appear(GridContent),
              frame1.ScaleTo(1, 300, Easing.Linear),
              StackContenido.TranslateTo(0, 0, 300, Easing.Linear),
              AreaResultados.TranslateTo(0, 0, 600, Easing.Linear)
           );


        }

        async void GestureVolver_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblVolver);
            await Animacion.Deseleccionar(lblVolver);

            await Task.WhenAll(
              StackContenido.TranslateTo(0, 100, 300, Easing.Linear)
           );

            await Navigation.PopAsync(false);
        }
    }
}

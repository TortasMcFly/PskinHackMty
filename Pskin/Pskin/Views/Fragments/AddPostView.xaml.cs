using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Pskin.API;
using Pskin.Models;
using Pskin.Utils;
using Xamarin.Forms;

namespace Pskin.Views.Fragments
{
    public partial class AddPostView : ContentView
    {
        byte[] imagen;
        Page context;
        Prediction prediction;

        public AddPostView( Page context )
        {
            InitializeComponent();

            //Evento tap Frame Post
            var gestureAnalizar = new TapGestureRecognizer();
            gestureAnalizar.Tapped += GestureAnalizar_Tapped;;
            BtnAnalizar.GestureRecognizers.Add(gestureAnalizar);

            AreaResultados.Opacity = 0;
            this.context = context;
        }

        async void Subir_Clicked(object sender, System.EventArgs e)
        {

            imagen = await Camara.TomarFoto();

            if (imagen != null)
            {
                BtnAnalizar.IsVisible = true;
                iconPlus.IsVisible = false;
                AreaResultados.Opacity = 0;
                lblAnalizar.Text = "Analizar";
                Image.Opacity = 1;

                var stream = new MemoryStream(imagen);
                ImageSource retSource = ImageSource.FromStream(() => stream);

                Image.Source = retSource;
            }


        }

        async void Galeria_Clicked(object sender, System.EventArgs e)
        {

            imagen = await Camara.ElegirFoto();

            if (imagen != null)
            {
                BtnAnalizar.IsVisible = true;
                iconPlus.IsVisible = false;
                AreaResultados.Opacity = 0;
                lblAnalizar.Text = "Analizar";
                Image.Opacity = 1;

                var stream = new MemoryStream(imagen);
                ImageSource retSource = ImageSource.FromStream(() => stream);

                Image.Source = retSource;
            }


        }

        async void GestureAnalizar_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(BtnAnalizar);
            await Animacion.Deseleccionar(BtnAnalizar);

            if(lblAnalizar.Text == "Guardar Análisis")
            {
                Analisis analisis = new Analisis
                {
                    Probabilidad = prediction.probability,
                    Tag = prediction.tagName,
                    ImagenBytes = imagen
                };

                Usuario user = JsonConvert.DeserializeObject<Usuario>(Application.Current.Properties["Usuario"].ToString());
                if (await PskinAPI.SaveAnalisis(analisis, user.Id))
                {
                    await context.DisplayAlert("Atención", "Se guardó el análisis correctamente", "Aceptar");
                    await Animacion.Disappear(AreaResultados);
                    Image.Source = "footer.png";
                    lblAnalizar.Text = "Analizar";
                    BtnAnalizar.IsVisible = false;
                    iconPlus.IsVisible = true;
                }
                else
                    await context.DisplayAlert("Ocurrió un error", "Vuelve a intentarlo", "Aceptar");

                return;
            }

            lblAnalizar.Text = "Analizando...";

            var stream = new MemoryStream(imagen);
            prediction = await Analizador.ClasificarImagen(stream);

            if (prediction != null)
            {
                lblTag.Text = prediction.tagName;
                lblPorcentaje.Text = (prediction.probability * 100).ToString("N") + "%";

                lblAnalizar.Text = "Guardar Análisis";
                AreaResultados.IsVisible = true;
                await Animacion.Appear(AreaResultados);
            }
            else
                lblAnalizar.Text = "Analizar";

        }

    }
}

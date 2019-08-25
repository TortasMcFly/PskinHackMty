using System;
using System.Collections.Generic;
using Pskin.Models;
using Pskin.Utils;
using Xamarin.Forms;

namespace Pskin.Views.Templates
{
    public partial class HistoriaTemplate : ContentView
    {
        Historia historia;
        public HistoriaTemplate(Historia historia)
        {
            InitializeComponent();

            //Gesture more
            var gestureMore = new TapGestureRecognizer();
            gestureMore.Tapped += GestureMore_Tapped;
            More.GestureRecognizers.Add(gestureMore);

            this.historia = historia;

            profile.Source = historia.Usuario.UrlImagen;
            username.Text = historia.Usuario.Nombre +" "+ historia.Usuario.Apellido;
            Image.Source = new UriImageSource
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

        async void GestureMore_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(More);
            await Animacion.Deseleccionar(More);
        }

    }
}

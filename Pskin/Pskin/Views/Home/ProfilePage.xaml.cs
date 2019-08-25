using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pskin.Models;
using Pskin.Utils;
using Pskin.Views.Templates;
using Xamarin.Forms;

namespace Pskin.Views.Home
{
    public partial class ProfilePage : ContentPage
    {
        Usuario user;
        public ProfilePage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            var gestureVolver = new TapGestureRecognizer();
            gestureVolver.Tapped += GestureVolver_Tapped;
            lblVolver.GestureRecognizers.Add(gestureVolver);

            //GridContent.Opacity = 0;
            GetUser();
            PopulateFotos();
        }

        private void GetUser()
        {
            user = JsonConvert.DeserializeObject<Usuario>(Application.Current.Properties["Usuario"].ToString());
            profile.Source = new UriImageSource
            {
                Uri = new Uri(user.UrlImagen),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0)
            };

            lblName.Text = user.Nombre + " " + user.Apellido;
            lblUsername.Text = user.Username;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await Task.WhenAll(
              //Animacion.Appear(GridContent),
              StackAnim.TranslateTo(0, 0, 300, Easing.Linear),
              profile.TranslateTo(0, 0, 300, Easing.Linear),
              profile.ScaleTo(1, 300, Easing.Linear),
              lblName.ScaleTo(1, 300, Easing.Linear),
              lblUsername.ScaleTo(1, 300, Easing.Linear),
              RightColumn.TranslateTo(0, 0, 600, Easing.Linear),
              LeftColumn.TranslateTo(0, 0, 600, Easing.Linear)
           );


        }

        async void GestureVolver_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblVolver);
            await Animacion.Deseleccionar(lblVolver);


            await Navigation.PopAsync(false);
        }

        private void PopulateFotos()
        {
            List<Foto> imagenes = user.Fotos;

            if(imagenes == null)
            {
                noInfo.IsVisible = true;
                return;
            }

            if (imagenes.Count == 0)
            {
                noInfo.IsVisible = true;
                return;
            }

            var column = LeftColumn;
            var postTapGestureRecognizer = new TapGestureRecognizer();
            postTapGestureRecognizer.Tapped += PostTapGestureRecognizer_Tapped; // Para agregar el tap a cada elemento

            bool left = true;

            for (var i = 0; i < imagenes.Count; i++)
            {
                var item = new PostTemplate();

                if (left)
                {
                    column = LeftColumn;
                    left = false;
                }
                else
                {
                    column = RightColumn;
                    left = true;
                }

                item.BindingContext = imagenes[i];
                item.GestureRecognizers.Add(postTapGestureRecognizer);
                item.PostImage.Source = new UriImageSource
                {
                    Uri = new Uri(imagenes[i].UrlImagen),
                    CachingEnabled = false
                };

                column.Children.Add(item);
            }
        }

        async void PostTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            PostTemplate post = (PostTemplate)sender;

            await Animacion.Seleccionar(post);
            await Animacion.Deseleccionar(post);


            Foto f = (Foto)post.BindingContext;

            /*if (f.EsAnalisis)
            {

            }
            else
            {

            }*/

        }

    }
}

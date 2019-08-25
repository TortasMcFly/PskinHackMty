using Newtonsoft.Json;
using Pskin.API;
using Pskin.Models;
using Pskin.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pskin.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CambiarFotoPage : ContentPage
    {
        byte[] imagen;
        Usuario user;
        public CambiarFotoPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            string json = Application.Current.Properties["Usuario"].ToString();
            user = JsonConvert.DeserializeObject<Usuario>(json);

            lblHolaUser.Text = "Hola " + user.Nombre;
            profileImage.Source = new UriImageSource
            {
                Uri = new Uri(user.UrlImagen),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0)
            };

            var gestureVolver = new TapGestureRecognizer();
            gestureVolver.Tapped += GestureVolver_Tapped;
            lblVolver.GestureRecognizers.Add(gestureVolver);
        }

        async void GestureVolver_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblVolver);
            await Animacion.Deseleccionar(lblVolver);


            await Navigation.PopAsync();
        }

        async void Subir_Clicked(object sender, System.EventArgs e)
        {
            imagen = await Camara.TomarFoto();

            if (imagen != null)
            {
                var stream = new MemoryStream(imagen);
                ImageSource retSource = ImageSource.FromStream(() => stream);

                profileImage.Source = retSource;

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


            }
        }

        async void Guardar_Clicked(object sender, System.EventArgs e)
        {
            if(imagen == null)
            {
                await DisplayAlert("Atención", "Selecciona una imágen nueva para tu perfil", "Aceptar");
            }
            else if (await PskinAPI.SubirImagen(imagen, user.Id))
            {
                await DisplayAlert("¡Bien hecho!", "Se actualizó tu imagen de perfil", "Aceptar");
            }
            else
            {
                await DisplayAlert("Ocurrió un error", "Vuelve a intentarlo", "Aceptar");
            }
        }
    }
}
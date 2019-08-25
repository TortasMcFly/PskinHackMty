using Newtonsoft.Json;
using Pskin.API;
using Pskin.Models;
using Pskin.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pskin.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        Usuario user;
        public EditProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            var gestureVolver = new TapGestureRecognizer();
            gestureVolver.Tapped += GestureVolver_Tapped;
            lblVolver.GestureRecognizers.Add(gestureVolver);

            string json = Application.Current.Properties["Usuario"].ToString();
            user = JsonConvert.DeserializeObject<Usuario>(json);

            NombreUsuario.Text = user.Nombre;
            ApellidoUsuario.Text = user.Apellido;
            EmailUsuario.Text = user.Email;

            profile.Source = new UriImageSource
            {
                Uri = new Uri(user.UrlImagen),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0)
            };

        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CambiarFotoPage());
        }

        async void Guardar_Clicked(object sender, System.EventArgs e)
        {
             user = new Usuario
            {
                Nombre = NombreUsuario.Text,
                Apellido = ApellidoUsuario.Text,
                Email = EmailUsuario.Text,
                Id = user.Id
            };

            string jsonUsr = JsonConvert.SerializeObject(user);

            if (await PskinAPI.EditarPerfil(jsonUsr))
            {
                await DisplayAlert("Editar perfil", "Sus cambios se han realizado con exito", "Aceptar");
            }
            else
            {
                await DisplayAlert("Ocurró un error", "Vuelve a intentarlo", "Aceptar");
            }


        }

        async void GestureVolver_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblVolver);
            await Animacion.Deseleccionar(lblVolver);


            await Navigation.PopAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pskin.API;
using Pskin.Models;
using Pskin.Utils;
using Pskin.Views.Home;
using Xamarin.Forms;

namespace Pskin.Views.Login
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //Establecer la opacidad de la vista en cero
            StackContent.Opacity = 0;

            var gestureVolver = new TapGestureRecognizer();
            gestureVolver.Tapped += GestureVolver_Tapped;
            lblVolver.GestureRecognizers.Add(gestureVolver);

            //Agregar evento tap al label
            var gestureLogin = new TapGestureRecognizer();
            gestureLogin.Tapped += Login;
            lblIniciarSesion.GestureRecognizers.Add(gestureLogin);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Iniciar animación
            await Task.WhenAll(
               Animacion.Appear(StackContent)
            );
        }

        async void GestureVolver_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblVolver);
            await Animacion.Deseleccionar(lblVolver);

            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        //Iniciar sesion
        async void Login(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblYaTengoCuenta);
            await Animacion.Deseleccionar(lblYaTengoCuenta);

            await Animacion.Disappear(GridContent);
            await Navigation.PushAsync(new LoginPage(), false);
        }


        async void Registrar_Clicked(object sender, System.EventArgs e)
        {
            Usuario user = new Usuario
            {
                Nombre = Nombre.Text,
                Apellido = Apellido.Text,
                Email = Email.Text,
                Pass = Pass.Text,
                Username =  BuildUsername(Email.Text)
            };

            string jsonUsr = JsonConvert.SerializeObject(user);

            if (await PskinAPI.Register(jsonUsr))
            {
                await Animacion.Disappear(GridContent);
                Application.Current.MainPage = new NavigationPage(new SubirFotoPage());
            }
            else 
            {
                await DisplayAlert("Ocurró un error", "Vuelve a intentarlo", "Aceptar");    
            }
        }

        private string BuildUsername(string mail)
        {
            string username = "";
            for (int i = 0; i < mail.Length; i++)
            {
                if (mail[i] == '@')
                    break;

                username += mail[i];
            }

            return username;
        }
    }
}

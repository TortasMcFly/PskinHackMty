using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pskin.Utils;
using Pskin.Views.Login;
using Xamarin.Forms;

namespace Pskin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //Establecer la opacidad de la vista en cero
            GridContent.Opacity = 0;

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
               Animacion.Appear(GridContent)
            );
        }

        //Iniciar sesion
        async void Login(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblYaTengoCuenta);
            await Animacion.Deseleccionar(lblYaTengoCuenta);

            await Animacion.Disappear(GridContent);
            await Navigation.PushAsync(new LoginPage(), false);
        }

        //Registrar
        async void Register(object sender, System.EventArgs e)
        {
            await Animacion.Disappear(GridContent);
            await Navigation.PushAsync(new RegisterPage(), false);
        }
    }
}

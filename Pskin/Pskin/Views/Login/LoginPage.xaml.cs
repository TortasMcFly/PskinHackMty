using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pskin.API;
using Pskin.Utils;
using Pskin.Views.Home;
using Xamarin.Forms;

namespace Pskin.Views.Login
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            //Establecer la opacidad de la vista en cero
            StackContent.Opacity = 0;

            var gestureVolver = new TapGestureRecognizer();
            gestureVolver.Tapped += GestureVolver_Tapped;
            lblVolver.GestureRecognizers.Add(gestureVolver);

            //Agregar evento tap al label
            var gestureRegister = new TapGestureRecognizer();
            gestureRegister.Tapped += Register;
            lblRegistrate.GestureRecognizers.Add(gestureRegister);
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

        async void Login(object sender, System.EventArgs e)
        {
            #region validaciones
            lblError.Text = "No puedes dejar campos vacíos";

            if (string.IsNullOrEmpty(email.Text))
            {
                email.Focus();
                await Animacion.Appear(lblError);
                return;
            }

            if (!Util.IsEmail(email.Text))
            {
                email.Focus();

                lblError.Text = "Correo inválido";
                await Animacion.Appear(lblError);
                return;
            }

            if (string.IsNullOrEmpty(pass.Text))
            {
                pass.Focus();
                await Animacion.Appear(lblError);
                return;
            }

            #endregion

            if(await PskinAPI.Login(email.Text, pass.Text))
            {
                await Animacion.Disappear(GridContent);
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                lblError.Text = "Correo o contraseña incorrectos";
                await Animacion.Appear(lblError);
            }

        }

        //Iniciar sesion
        async void Register(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(lblNuevaCuenta);
            await Animacion.Deseleccionar(lblNuevaCuenta);

            await Animacion.Disappear(GridContent);
            await Navigation.PushAsync(new RegisterPage(), false);
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            lblError.Opacity = 0;
        }
    }
}

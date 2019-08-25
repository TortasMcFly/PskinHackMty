using System;
using Pskin.Views.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Pskin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var isLoggedIn = Properties.ContainsKey("IsLoggedIn") && (bool)Properties["IsLoggedIn"];

            if (isLoggedIn)
            {
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

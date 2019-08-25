using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pskin.Models;
using Pskin.Utils;
using Pskin.Views.Fragments;
using Xamarin.Forms;

namespace Pskin.Views.Home
{
    public partial class HomePage : ContentPage
    {
        //1 home, 2 add, 3 user
        private int pointSelected = 1;
        public HomePage()
        {
            InitializeComponent();
            Application.Current.Properties["VieneDePostear"] = false;
            Application.Current.SavePropertiesAsync();
            Init();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if(Application.Current.Properties.ContainsKey("VieneDePostear") && (bool)Application.Current.Properties["VieneDePostear"])
            {
                //Seleccionar por defecto el home
                StackContent.Children.Clear();
                StackContent.Children.Add(new HistoriasView());


                Application.Current.Properties["VieneDePostear"] = false;
                await Application.Current.SavePropertiesAsync();
            }
            else
            {
                //Iniciar animación
                await Task.WhenAll(
                   Animacion.Appear(GridContent)
                );
            }
           
        }

        #region Init
        private void Init()
        {
            //Esconder la barra de navegación
            NavigationPage.SetHasNavigationBar(this, false);

            //Establecer la opacidad de la vista en cero
            GridContent.Opacity = 0;

            //Evento tap Search
            var gestureSearch = new TapGestureRecognizer();
            gestureSearch.Tapped += GestureSearch_Tapped; 
            search.GestureRecognizers.Add(gestureSearch);

            //Evento tap Profile
            var gestureProfile = new TapGestureRecognizer();
            gestureProfile.Tapped += GestureProfile_Tapped; 
            profile.GestureRecognizers.Add(gestureProfile);

            //Evento tap HOME
            var gestureHome = new TapGestureRecognizer();
            gestureHome.Tapped += GestureHome_Tapped;
            home.GestureRecognizers.Add(gestureHome);

            //Evento tap HOME
            var gestureAdd = new TapGestureRecognizer();
            gestureAdd.Tapped += GestureAdd_Tapped;
            add.GestureRecognizers.Add(gestureAdd);

            //Evento tap HOME
            var gestureFavoritos = new TapGestureRecognizer();
            gestureFavoritos.Tapped += GestureFav_Tapped;
            favoritos.GestureRecognizers.Add(gestureFavoritos);

            //Seleccionar por defecto el home
            StackContent.Children.Clear();
            StackContent.Children.Add(new HistoriasView());

            GetUser();
        }

        private void GetUser()
        {
            Usuario user = JsonConvert.DeserializeObject<Usuario>(Application.Current.Properties["Usuario"].ToString());
            
            profile.Source = new UriImageSource
            {
                Uri = new Uri(user.UrlImagen),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0)
            };
        }

        #endregion

        #region NavigationBar
        async void Menu_Clicked(object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Cerrar Sesión", "¿Estás seguro de cerrar sesión?", "Aceptar", "Cancelar"))
            {
                Application.Current.Properties["Usuario"] = "";
                Application.Current.Properties["IsLoggedIn"] = false;
                await Application.Current.SavePropertiesAsync();
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }

        async void GestureSearch_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(search);
            await Animacion.Deseleccionar(search);
        }


        async void GestureProfile_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(profile);
            await Animacion.Deseleccionar(profile);

            await Navigation.PushAsync(new ProfilePage(), false);

            GridContent.Opacity = 0;
        }

        #endregion

        #region TabBar
        async void GestureHome_Tapped(object sender, EventArgs e)
        {
            StackContent.Children.Clear();
            StackContent.Children.Add(new HistoriasView());
            await Task.WhenAll(StartHome());
        }

        async void GestureAdd_Tapped(object sender, EventArgs e)
        {
            StackContent.Children.Clear();
            StackContent.Children.Add(new AddPostView(this));
            await Task.WhenAll(StartAdd());
        }

        async void GestureFav_Tapped(object sender, EventArgs e)
        {
            StackContent.Children.Clear();
            StackContent.Children.Add(new FavoritesView());
            await Task.WhenAll(StartFavoritos());
        }


        #endregion

        #region animacion TabBar
        private async Task StartHome()
        {

            if (pointSelected == 1)
                return;

            if (pointSelected == 2)
            {
                await pointAdd.TranslateTo(-50, 0, 100, Easing.Linear);
                pointAdd.IsVisible = false;
            }
            else if (pointSelected == 3)
            {
                await pointFavoritos.TranslateTo(-100, 0, 100, Easing.Linear);
                pointFavoritos.IsVisible = false;
            }

            pointSelected = 1;
            pointHome.IsVisible = true;

            pointHome.TranslationX = 0;
            pointAdd.TranslationX = 0;
            pointFavoritos.TranslationX = 0;
        }

        private async Task StartAdd()
        {

            if (pointSelected == 2)
                return;

            if (pointSelected == 1)
            {
                await pointHome.TranslateTo(50, 0, 100, Easing.Linear);
                pointHome.IsVisible = false;
            }
            else if (pointSelected == 3)
            {
                await pointFavoritos.TranslateTo(-50, 0, 100, Easing.Linear);
                pointFavoritos.IsVisible = false;
            }

            pointSelected = 2;
            pointAdd.IsVisible = true;

            pointHome.TranslationX = 0;
            pointAdd.TranslationX = 0;
            pointFavoritos.TranslationX = 0;
        }

        private async Task StartFavoritos()
        {

            if (pointSelected == 3)
                return;

            if (pointSelected == 1)
            {
                await pointHome.TranslateTo(100, 0, 100, Easing.Linear);
                pointHome.IsVisible = false;
            }
            else if (pointSelected == 2)
            {
                await pointAdd.TranslateTo(50, 0, 100, Easing.Linear);
                pointAdd.IsVisible = false;
            }

            pointSelected = 3;
            pointFavoritos.IsVisible = true;

            pointHome.TranslationX = 0;
            pointAdd.TranslationX = 0;
            pointFavoritos.TranslationX = 0;
        }
        #endregion

        /* Evento para el scroll infinito
        async void Handle_Scrolled(object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            if (scrollingSpace <= e.ScrollY && !isScrolling) // Touched bottom
            {
                isScrolling = true;

                // Do the things you want to do
                string opc = await DisplayActionSheet("Hola perro", "Aceptar", "Cancelar", "NEL");
                if (opc == "NEL")
                    isScrolling = false;
                else
                    isScrolling = false;
            }
        }       
        */
    }
}

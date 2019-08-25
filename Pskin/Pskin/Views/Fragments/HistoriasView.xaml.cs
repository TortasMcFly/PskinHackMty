using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pskin.API;
using Pskin.Models;
using Pskin.Utils;
using Pskin.Views.Home;
using Pskin.Views.Templates;
using Xamarin.Forms;

namespace Pskin.Views.Fragments
{
    public partial class HistoriasView : ContentView
    {
        public HistoriasView()
        {
            InitializeComponent();

            //Evento tap Frame Post
            var gesturePost = new TapGestureRecognizer();
            gesturePost.Tapped += GesturePost_Tapped;
            FramePost.GestureRecognizers.Add(gesturePost);

            //Evento tap Label Post
            var gestureCameraPost = new TapGestureRecognizer();
            gestureCameraPost.Tapped += GestureCameraPost_Tapped; 
            cameraPost.GestureRecognizers.Add(gestureCameraPost);

            Device.BeginInvokeOnMainThread( async () =>
            {

                List<Historia> historias = await PskinAPI.GetAllHistorias();

                await Animacion.Disappear(StackContent);
                StackContent.Children.Clear();

                if (historias != null)
                {
                    PopulateHistorias(historias);
                }
                else if (Application.Current.Properties.ContainsKey("Historias"))
                {
                    historias = JsonConvert.DeserializeObject<List<Historia>>(Application.Current.Properties["Historias"].ToString());
                    if(historias != null) PopulateHistorias(historias);
                }

                await Animacion.Appear(StackContent);


                return;
            });
        }



        #region Main
        async void GesturePost_Tapped(object sender, System.EventArgs e)
        {
            await Animacion.Seleccionar(FramePost);
            await Animacion.Deseleccionar(FramePost);

            Application.Current.Properties["VieneDePostear"] = true;
            await Application.Current.SavePropertiesAsync();

            await Navigation.PushModalAsync(new NavigationPage(new NuevaHistoriaPage(false)));

        }

        async void GestureCameraPost_Tapped(object sender, EventArgs e)
        {
            await Animacion.Seleccionar(cameraPost);
            await Animacion.Deseleccionar(cameraPost);

            Application.Current.Properties["VieneDePostear"] = true;
            await Application.Current.SavePropertiesAsync();

            await Navigation.PushModalAsync(new NavigationPage(new NuevaHistoriaPage(true)));
        }
        #endregion

        void PopulateHistorias(List<Historia> historias)
        {

            var postTapGestureRecognizer = new TapGestureRecognizer();
            postTapGestureRecognizer.Tapped += PostTapGestureRecognizer_Tapped;  // Para agregar el tap a cada elemento

            foreach(Historia h in historias)
            {
                var item = new HistoriaTemplate(h);
                item.BindingContext = h;
                item.GestureRecognizers.Add(postTapGestureRecognizer);
                StackContent.Children.Add(item);
            }
        }


        async void PostTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            HistoriaTemplate post = (HistoriaTemplate)sender;

            Historia hSelected = (Historia)post.BindingContext;
            Application.Current.Properties["Historia"] = JsonConvert.SerializeObject(hSelected);
            await Application.Current.SavePropertiesAsync();


            await Navigation.PushAsync(new HistoriaPage(), false);
        }
    }
}

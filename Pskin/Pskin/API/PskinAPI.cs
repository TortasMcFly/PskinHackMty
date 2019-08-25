using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pskin.Models;
using Xamarin.Forms;

namespace Pskin.API
{
    public static class PskinAPI
    {
        static HttpClient Client = new HttpClient(new HttpClientHandler());
        public const string Urlbase = "https://pskinapi-ik7.conveyor.cloud/api/";
        public const string UrlImage = "https://pskinapi-ik7.conveyor.cloud/Imagenes/";


        //-----Registro--------
        //Info
        public static async Task<bool> Register(string jsonUsr)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(Urlbase + "Usuarios/Registro"));
                request.Headers.Add("Usuario", jsonUsr);

                var response = await Client.SendAsync(request);
                string Json = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    return false;


                Debug.WriteLine(Json);

                Application.Current.Properties["Usuario"] = Json;
                Application.Current.Properties["IsLoggedIn"] = true;
                await Application.Current.SavePropertiesAsync();

                return true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en el login: " + ex.ToString());
                return false;
            }
        }

        //Foto perfil
        public static async Task<bool> SubirImagen(byte[] imagen, int idUsuario)
        {
            var url = Urlbase + "Usuarios/SubirImagen";

            try
            {
                Debug.WriteLine("INICIO A SUBIRSE AL SERVIDOR");

                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(imagen);

                content.Add(baContent, "File", string.Format("{0}.jpg", Guid.NewGuid()));
                content.Headers.Add("IdUsuario", idUsuario.ToString());

                var response = await Client.PostAsync(url, content);

                var Json = response.Content.ReadAsStringAsync().Result;

                Application.Current.Properties["Usuario"] = Json;
                await Application.Current.SavePropertiesAsync();

                Debug.WriteLine(Json);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception Caught: " + e.ToString());
                return false;
            }
        }

        //-----Login----------
        //Inicar Sesión
        public static async Task<bool> Login(string email, string pass)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(Urlbase + "Usuarios/Login"));
            request.Headers.Add("Email", email);
            request.Headers.Add("Pass", pass);

            try
            {

                var response = await Client.SendAsync(request);
                string Json = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    return false;


                Debug.WriteLine(Json);

                Application.Current.Properties["Usuario"] = Json;
                Application.Current.Properties["IsLoggedIn"] = true;
                await Application.Current.SavePropertiesAsync();

                return true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en el login: " + ex.ToString());
                return false;
            }
        }

        //-----Inicio---------
        //Lista de historias
        public static async Task<List<Historia>> GetAllHistorias()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(Urlbase + "Historias/GetAll"));

            try
            {

                var response = await Client.SendAsync(request);
                string Json = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                    return null;


                Debug.WriteLine(Json);
                Application.Current.Properties["Historias"] = Json;
                await Application.Current.SavePropertiesAsync();

                return JsonConvert.DeserializeObject<List<Historia>>(Json); 

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en el login: " + ex.ToString());
                return null;
            }
        }
        //Subir historia
        public static async Task<bool> SubirHistoria(byte[] imagen, int idUsuario, string descripcion)
        {
            var url = Urlbase + "Historias/GuardarHistoria";

            try
            {
                Debug.WriteLine("INICIO A SUBIRSE AL SERVIDOR");

                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(imagen);

                content.Add(baContent, "File", string.Format("{0}.jpg", Guid.NewGuid()));
                content.Headers.Add("IdUsuario", idUsuario.ToString());
                content.Headers.Add("Descripcion", descripcion);

                var response = await Client.PostAsync(url, content);

                var Json = response.Content.ReadAsStringAsync().Result;

                Application.Current.Properties["Usuario"] = Json;
                await Application.Current.SavePropertiesAsync();

                Debug.WriteLine(Json);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception Caught: " + e.ToString());
                return false;
            }
        }

        //-----Análisis--------
        //Guardar análisis
        public static async Task<bool> SaveAnalisis(Analisis analisis, int idUsuario)
        {
            var url = Urlbase + "Analisis/GuardarAnalisis";

            try
            {
                Debug.WriteLine("INICIO A SUBIRSE AL SERVIDOR");

                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(analisis.ImagenBytes);

                content.Add(baContent, "File", string.Format("{0}.jpg", Guid.NewGuid()));
                content.Headers.Add("Tag", analisis.Tag);
                content.Headers.Add("Probabilidad", analisis.Probabilidad.ToString());
                content.Headers.Add("IdUsuario", idUsuario.ToString());

                var response = await Client.PostAsync(url, content);

                var Json = response.Content.ReadAsStringAsync().Result;

                Application.Current.Properties["Usuario"] = Json;
                await Application.Current.SavePropertiesAsync();

                Debug.WriteLine(Json);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception Caught: " + e.ToString());
                return false;
            }
        }
        //Eliminar análisis

        //-----Perfil--------
        //Editar perfil
        //Cambiar imágen


    }
}

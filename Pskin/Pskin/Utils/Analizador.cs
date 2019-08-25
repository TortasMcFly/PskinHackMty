using Newtonsoft.Json;
using Pskin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pskin.Utils
{
    public static class Analizador
    {
        public async static Task<Prediction> ClasificarImagen(MemoryStream stream)
        {
            try
            {
                var url = Constantes.PredictionURL;

                using (var cliente = new HttpClient())
                {
                    cliente.DefaultRequestHeaders.Add("Prediction-Key", Constantes.PredictionKey);

                    using (var content = new ByteArrayContent(stream.ToArray()))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                        var post = await cliente.PostAsync(url, content);
                        var resultado = await post.Content.ReadAsStringAsync();

                        var cv = JsonConvert.DeserializeObject<CustomVisionResult>(resultado);

                        if (cv.predictions.Length > 0)
                        {
                            var prediccion = ObtenerPrediccion(cv);
                            return prediccion;
                        }
                        else

                            return null;
                    }
                }

            }
            catch (Exception ex) { return null; }
        }

        static Prediction ObtenerPrediccion(CustomVisionResult cv)
        {
            return cv.predictions.OrderByDescending(x => x.probability).Take(1).First();
        }
    }

    public static class Constantes
    {
        public const string PredictionKey = "540b47edfcc94e769ac257f2f095ff8b";
        public const string PredictionURL = "https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/2d35a3d1-c72f-43e3-b720-98029089ad54/image?iterationId=c0f044a9-6edf-4228-93d8-e97119239e26";
    }
}

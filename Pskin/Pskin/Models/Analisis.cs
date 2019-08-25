using System;
using Pskin.API;
using Pskin.Utils;

namespace Pskin.Models
{
    public class Analisis
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public double Probabilidad { get; set; }
        public string ImagenUrl { get; set; }
        public string UrlImagen
        {
            get
            {
                return PskinAPI.UrlImage + ImagenUrl;
            }
        }
        public DateTime Fecha { get; set; }
        public byte[] ImagenBytes { get; set; }

        public string TimeAgo
        {
            get
            {
                return Util.TimeAgo(Fecha);
            }
        }
    }
}

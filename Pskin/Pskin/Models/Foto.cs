using System;
using Pskin.API;

namespace Pskin.Models
{
    public class Foto
    {
        public int Id { get; set; }
        public bool EsAnalisis { get; set; }
        public string ImagenUrl { get; set; }
        public string UrlImagen
        {
            get
            {
                return PskinAPI.UrlImage + ImagenUrl;
            }
        }
    }
}

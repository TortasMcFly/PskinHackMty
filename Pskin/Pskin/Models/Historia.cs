using System;
using Pskin.API;
using Pskin.Utils;

namespace Pskin.Models
{
    public class Historia
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public string ImagenUrl { get; set; }
        public string UrlImagen
        {
            get
            {
                return PskinAPI.UrlImage + ImagenUrl;
            }
        }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public int Likes { get; set; }
        public bool TieneLike { get; set; }

        public string TimeAgo
        {
            get
            {
                return Util.TimeAgo(Fecha);
            }
        }

    }
}

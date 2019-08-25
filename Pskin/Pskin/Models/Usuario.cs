using System;
using System.Collections.Generic;
using Pskin.API;

namespace Pskin.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Username { get; set; }
        public string FotoUrl { get; set; }

        public string UrlImagen
        {
            get
            {
                if (string.IsNullOrEmpty(FotoUrl))
                {
                    return "usrprofile.jpg";
                }
                else
                {
                    return PskinAPI.UrlImage + FotoUrl;
                }
            }
        }
        public List<Foto> Fotos { get; set; }
        public Galeria Galeria { get; set; }

    }
}

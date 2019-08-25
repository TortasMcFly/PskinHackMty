using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PskinAPI.Models
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
        public Galeria Galeria { get; set; }
        public List<Foto> Fotos { get; set; }
    }
}
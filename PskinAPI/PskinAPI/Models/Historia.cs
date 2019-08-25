using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PskinAPI.Models
{
    public class Historia
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public string ImagenUrl { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public int Likes { get; set; }
        public bool TieneLike { get; set; }
    }
}
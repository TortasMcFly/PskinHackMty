using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PskinAPI.Models
{
    public class Analisis
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public double Probabilidad { get; set; }
        public string ImagenUrl { get; set; }
        public int idUsuario { get; set; }
        public DateTime Fecha { get; set; }
    }
}
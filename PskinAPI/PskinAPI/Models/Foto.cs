using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PskinAPI.Models
{
    public class Foto
    {
        public int Id { get; set; }
        public bool EsAnalisis { get; set; }
        public string ImagenUrl { get; set; }

        public DateTime Fecha { get; set; }
    }
}
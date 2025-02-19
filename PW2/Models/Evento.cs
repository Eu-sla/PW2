using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW2.Models
{
    public class Evento
    {
        public string Local { get; set; }
        public DateTime Data { get; set; }

        public static List<Evento> GerarLista()
        {
            var lista = new List<Evento>();
            lista.Add(new Evento() { Local = "Prefeitura", Data = 12 / 02 / 2026 - 16:00 }
        }
    }
}
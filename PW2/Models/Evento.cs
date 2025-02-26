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
            lista.Add(new Evento() { Local = "Prefeitura", Data = new DateTime(2026,02,12,14,30,00) });
            lista.Add(new Evento() { Local = "Escola", Data = new DateTime(2023,05,10,16,30,00) });
            lista.Add(new Evento() { Local = "Prefeitura", Data = new DateTime(2026,02,12,14,30,00) });
            return lista;
        }
    }
}
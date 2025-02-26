using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW2.Models
{
    public class Celular
    {
        public int Numero { get; set; }
        public string Marca { get; set; }
        public bool Novo { get; set; }

        public static List<Celular> GerarLista() 
        { 
            var lista = new List<Celular>();
            lista.Add(new Celular { Numero = 998553294, Marca="Samsung", Novo=false });
            lista.Add(new Celular { Numero = 647973556, Marca="Apple", Novo=true });
            lista.Add(new Celular { Numero = 996254789, Marca="Samsung", Novo=true });
            lista.Add(new Celular { Numero = 899563441, Marca="Xiaomi", Novo=false });

            return lista;
        }
    }
}
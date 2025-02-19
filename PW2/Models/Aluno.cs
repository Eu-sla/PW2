using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW2.Models
{
    public class Aluno
    {

        public string Nome { get; set; }
        public string RA { get; set; }

        public static List<Aluno> GerarLista()
        {
            var lista = new List<Aluno>();
            lista.Add(new Aluno { Nome = "Barbie", RA = "234567" });
            lista.Add(new Aluno { Nome = "sceds", RA = "325432" });
            lista.Add(new Aluno { Nome = "alfred", RA = "182732" });

            return lista;
        }
    }
}
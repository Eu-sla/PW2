using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW2.Models
{
    public class Carro
    {
        public string Placa { get; set; }
        public int Ano { get; set; }
        public string Cor { get; set; }

        public static List<Carro> GerarLista()
        {
            var carro = new List<Carro>();
            carro.Add(new Carro { Placa = "DFR-1928", Ano = 2020, Cor = "Preto" });
            carro.Add(new Carro { Placa = "WJA-9261", Ano = 2023, Cor = "Cinza" });
            carro.Add(new Carro { Placa = "PEP-6342", Ano = 2018, Cor = "Prata" });
            carro.Add(new Carro { Placa = "SCE-9163", Ano = 2002, Cor = "Vermelho" });

            return carro;
        }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                if (((List<Carro>)session["ListaCarro"]).Count > 0)
                {
                    return;
                }
            }
            var lista = new List<Aluno>();
            lista.Add(new Aluno { Nome = "Barbie", RA = "234567" });
            lista.Add(new Aluno { Nome = "sceds", RA = "325432" });
            lista.Add(new Aluno { Nome = "alfred", RA = "182732" });

            session.Remove("ListaCarro");
            session.Add("ListaCarro", lista);
        }
        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                (session["ListaCarro"] as List<Carro>).Add(this);
            }
        }
    }
}
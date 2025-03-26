using System;
using System.Collections;
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

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                if (((List<Carro>)session["ListaCarro"]).Count > 0)
                {
                    return;
                }
            }
            var carro = new List<Carro>();
            carro.Add(new Carro { Placa = "DFR-1928", Ano = 2020, Cor = "Preto" });
            carro.Add(new Carro { Placa = "WJA-9261", Ano = 2023, Cor = "Cinza" });
            carro.Add(new Carro { Placa = "PEP-6342", Ano = 2018, Cor = "Prata" });
            carro.Add(new Carro { Placa = "SCE-9163", Ano = 2002, Cor = "Vermelho" });
            session.Remove("ListaCarro");
            session.Add("ListaCarro", carro);
        }

        

        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                (session["ListaCarro"] as List<Carro>).Add(this);
            }
        }
        public static Carro Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                return (session["ListaCarro"] as List<Carro>).ElementAt(id);
            }
            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                (session["ListaCarro"] as List<Carro>).Remove(this);
            }
        }
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                var carro = Carro.Procurar(session, id);
                carro.Placa = this.Placa;
                carro.Ano = this.Ano;
                carro.Cor = this.Cor;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PW2.Models
{
    public class Carro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "O ano é obrigatório.")]
        public int Ano { get; set; }

        [Required(ErrorMessage = "A cor é obrigatória.")]
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

            var lista = new List<Carro>();
            lista.Add(new Carro { Id = 1, Placa = "ABC1234", Ano = 2010, Cor = "Preto" });
            lista.Add(new Carro { Id = 2, Placa = "XYZ5678", Ano = 2015, Cor = "Branco" });
            lista.Add(new Carro { Id = 3, Placa = "JKL9012", Ano = 2020, Cor = "Vermelho" });

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

        public static Carro Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                return (session["ListaCarro"] as List<Carro>).FirstOrDefault(c => c.Id == id);
            }
            return null;
        }

        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaCarro"] != null)
            {
                var lista = (session["ListaCarro"] as List<Carro>);
                var carro = lista.FirstOrDefault(c => c.Id == this.Id);
                if (carro != null)
                {
                    lista.Remove(carro);
                }
            }
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCarro"] != null)
            {
                var carro = Carro.Procurar(session, id);
                if (carro != null)
                {
                    carro.Placa = this.Placa;
                    carro.Ano = this.Ano;
                    carro.Cor = this.Cor;
                }
            }
        }
    }
}

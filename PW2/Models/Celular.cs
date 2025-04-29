using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW2.Models
{
    public class Celular
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string Marca { get; set; }
        public bool Novo { get; set; }

        // Gerar lista de celulares
        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                if (((List<Celular>)session["ListaCelular"]).Count > 0)
                {
                    return;
                }
            }
            var lista = new List<Celular>();
            lista.Add(new Celular { Id = 1, Numero = 998553294, Marca = "Samsung", Novo = false });
            lista.Add(new Celular { Id = 2, Numero = 647973556, Marca = "Apple", Novo = true });
            lista.Add(new Celular { Id = 3, Numero = 996254789, Marca = "Samsung", Novo = true });
            lista.Add(new Celular { Id = 4, Numero = 899563441, Marca = "Xiaomi", Novo = false });

            session.Remove("ListaCelular");
            session.Add("ListaCelular", lista);
        }

        // Adicionar celular à lista
        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                (session["ListaCelular"] as List<Celular>).Add(this);
            }
        }

        // Procurar celular por ID
        public static Celular Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCelular"] != null)
            {
                return (session["ListaCelular"] as List<Celular>).FirstOrDefault(c => c.Id == id);
            }
            return null;
        }

        // Excluir celular da lista
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaCelular"] != null)
            {
                var lista = (session["ListaCelular"] as List<Celular>);
                var celular = lista.FirstOrDefault(c => c.Id == this.Id);
                if (celular != null)
                {
                    lista.Remove(celular);
                }
            }
        }

        // Editar celular
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaCelular"] != null)
            {
                var celular = Celular.Procurar(session, id);
                if (celular != null)
                {
                    celular.Numero = this.Numero;
                    celular.Marca = this.Marca;
                    celular.Novo = this.Novo;
                }
            }
        }
    }
}

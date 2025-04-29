using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW2.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public DateTime Data { get; set; }

        // Gerar lista de eventos
        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] != null)
            {
                if (((List<Evento>)session["ListaEvento"]).Count > 0)
                {
                    return;
                }
            }
            var lista = new List<Evento>();
            lista.Add(new Evento() { Id = 1, Local = "Prefeitura", Data = new DateTime(2026, 02, 12, 14, 30, 00) });
            lista.Add(new Evento() { Id = 2, Local = "Escola", Data = new DateTime(2023, 05, 10, 16, 30, 00) });
            lista.Add(new Evento() { Id = 3, Local = "Prefeitura", Data = new DateTime(2026, 09, 11, 13, 00, 00) });

            session.Remove("ListaEvento");
            session.Add("ListaEvento", lista);
        }

        // Adicionar evento à lista
        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] != null)
            {
                (session["ListaEvento"] as List<Evento>).Add(this);
            }
        }

        // Procurar evento por ID
        public static Evento Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaEvento"] != null)
            {
                return (session["ListaEvento"] as List<Evento>).FirstOrDefault(e => e.Id == id);
            }
            return null;
        }

        // Excluir evento da lista
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] != null)
            {
                var lista = (session["ListaEvento"] as List<Evento>);
                var evento = lista.FirstOrDefault(e => e.Id == this.Id);
                if (evento != null)
                {
                    lista.Remove(evento);
                }
            }
        }

        // Editar evento
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaEvento"] != null)
            {
                var evento = Evento.Procurar(session, id);
                if (evento != null)
                {
                    evento.Local = this.Local;
                    evento.Data = this.Data;
                }
            }
        }
    }
}

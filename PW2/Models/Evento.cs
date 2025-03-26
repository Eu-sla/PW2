﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PW2.Models
{
    public class Evento
    {
        public string Local { get; set; }
        public DateTime Data { get; set; }

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
            lista.Add(new Evento() { Local = "Prefeitura", Data = new DateTime(2026,02,12,14,30,00) });
            lista.Add(new Evento() { Local = "Escola", Data = new DateTime(2023,05,10,16,30,00) });
            lista.Add(new Evento() { Local = "Prefeitura", Data = new DateTime(2026,09,11,13,00,00) });
            session.Remove("ListaEvento");
            session.Add("ListaEvento", lista);
        }
        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] != null)
            {
                (session["ListaEvento"] as List<Evento>).Add(this);
            }
        }
        public static Evento Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaEvento"] != null)
            {
                return (session["ListaEvento"] as List<Evento>).ElementAt(id);
            }
            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaEvento"] != null)
            {
                (session["ListaEvento"] as List<Evento>).Remove(this);
            }
        }
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaEvento"] != null)
            {
                var evento = Evento.Procurar(session, id);
                evento.Local = this.Local;
                evento.Data = this.Data;
            }
        }
    }
}
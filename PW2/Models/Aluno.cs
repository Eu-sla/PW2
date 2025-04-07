using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;

namespace PW2.Models
{
    public class Aluno
    {

        public string Nome { get; set; }
        public string RA { get; set; }

        public DateTime DataNasc { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null)
            {
                if(((List<Aluno>)session["ListaAluno"]).Count > 0)
                {
                    return; 

                }
            }
            var lista = new List<Aluno>();
            lista.Add(new Aluno { Nome = "Barbie", RA = "234567" , DataNasc = new DateTime(1999, 02, 19) });
            lista.Add(new Aluno { Nome = "sceds", RA = "325432", DataNasc = new DateTime(2005, 12, 12) });
            lista.Add(new Aluno { Nome = "alfred", RA = "182732", DataNasc = new DateTime(2015, 06, 02) });

            session.Remove("ListaAluno");
            session.Add("ListaAluno", lista);
        }
        public void Adicionar(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null)
            {
                (session["ListaAluno"] as List<Aluno>).Add(this);
            }
        }
        public static Aluno Procurar(HttpSessionStateBase session, int id)
        {
            if (session["ListaAluno"] != null)
            {
                return (session["ListaAluno"] as List<Aluno>).ElementAt(id);
            }
            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null)
            {
                (session["ListaAluno"] as List<Aluno>).Remove(this);
            }
        }
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaAluno"] != null)
            {
                var aluno = Aluno.Procurar(session, id);
                aluno.Nome = this.Nome;
                aluno.RA = this.RA;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PW2.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O RA é obrigatório.")]
        public string RA { get; set; }
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
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
            lista.Add(new Aluno { Id = 1,Nome = "Barbie", RA = "234567" , DataNasc = new DateTime(1999, 02, 19) });
            lista.Add(new Aluno { Id= 2, Nome = "sceds", RA = "325432", DataNasc = new DateTime(2005, 12, 12) });
            lista.Add(new Aluno { Id = 3, Nome = "alfred", RA = "182732", DataNasc = new DateTime(2015, 06, 02) });

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
                return (session["ListaAluno"] as List<Aluno>).FirstOrDefault(a => a.Id == id);
            }
            return null;
        }
        public void Excluir(HttpSessionStateBase session)
        {
            if (session["ListaAluno"] != null)
            {
                var lista = (session["ListaAluno"] as List<Aluno>);
                var aluno = lista.FirstOrDefault(a => a.Id == this.Id);
                if (aluno != null)
                {
                    lista.Remove(aluno);
                }
            }
        }
        public void Editar(HttpSessionStateBase session, int id)
        {
            if (session["ListaAluno"] != null)
            {
                var aluno = Aluno.Procurar(session, id);
                if (aluno != null)
                {
                    aluno.Nome = this.Nome;
                    aluno.RA = this.RA;
                    aluno.DataNasc = this.DataNasc;
                }
            }
        }

    }
}
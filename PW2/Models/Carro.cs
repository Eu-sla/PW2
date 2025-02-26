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
    }
}
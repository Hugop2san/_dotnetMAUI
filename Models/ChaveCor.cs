using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kogui.Models
{
    public class ChaveCor //REQUISITO 1
    {
        public string Hex { get; set; }
        public string Cor { get; set; }
        public string Componente { get; set; } // frases



        public ChaveCor(string cor, string componente, string hex = null) 
        {
            Hex = hex;
            Cor = cor;    
            Componente = componente; 
        }



    }
}

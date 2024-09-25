using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Model
{
    public class Usuario
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public NivelEnum Nivel { get; set; }
        public enum NivelEnum
        {
            Master = 1,
            Gestora = 2,
            Consultoria = 3,
            Denver = 4
        }

        public Usuario(string email, string senha, NivelEnum nivel)
        {
            Email = email;Senha= senha;Nivel=nivel;
        }
    }
}

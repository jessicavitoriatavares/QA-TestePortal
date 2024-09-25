using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestePortal.Model;

namespace TestePortal.Utils
{
    public class Util
    {
        public static List<Usuario> GetUsuariosForTest()
        {
            var usuarios = new List<Usuario>();

            usuarios.Add(new Usuario("adm@idsf.com.br", "id2021",Usuario.NivelEnum.Master));
            usuarios.Add(new Usuario("jt@zitec.ai", "Jehtavares?123", Usuario.NivelEnum.Gestora));
            usuarios.Add(new Usuario("jessica.tavares@aluno.ifsp.edu.br", "Jehtavares?123", Usuario.NivelEnum.Consultoria));
            usuarios.Add(new Usuario("jessica.vitoria.tavares044@gmail.com", "Jehtavares?123", Usuario.NivelEnum.Denver));
            return usuarios;
        }

    }
}

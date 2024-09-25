using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Navigation;

namespace TestePortal.Model
{


    public class Pagina
    { 
        
        public string Nome { get; set; }
        public int StatusCode { get; set; }

       // public List<string> ListaErros { get; set; }
        public int TotalErros { get; set; }
        public string Listagem { get; set; }
        public string BaixarExcel { get; set; }
        public string InserirDados { get; set; }
        public string Excluir { get; set; }
        public string Reprovar { get; set; }
        public string Acentos { get; set; }
        public string Perfil { get; set;  }

        public string ToFormattedString(int padding = 20)
        {
            try
            {

                // String formatada com espaçamento personalizado
                return $"Nome: {Nome.PadRight(padding)}\n" +
                       $"StatusCode: {StatusCode.ToString().PadRight(padding)}\n";
                     
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                return null;
            }
        }

    }
}


using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TestePortal.Model;

namespace TestePortal.Utils
{
    public static class Acentos
    {
        public static async  Task<string> ValidarAcentos(IPage Page) {

          
            string Acentos = "";

            string[] textos = { "Ã£", "Ã§", "Ü", "Ã¡", "Ã©", "Ã¨", "Ã§Ã£", "Ã§Ã" };
            var contadores = new Dictionary<string, int>();
            var listErros = new List<string>();

            //Verificar e contar elementos para cada texto
            foreach (var texto in textos)
            {
                int count = await Page.GetByText(texto).CountAsync();
                contadores[texto] = count;
            }

            // Exibir os resultados
            foreach (var item in contadores)
            {
                if (item.Value > 0)
                {
                    Console.WriteLine($"O texto '{item.Key}' foi encontrado na página.");
                    Acentos = "❌";
                    listErros.Add("Erro ao baixar arquivo excel de acentuação");
                    break;
                }
                else
                {
                    Acentos = "✅";
                    Console.WriteLine($"Nenhum dos textos '{item.Key}' foi encontrado na página.");
                }
            }

            return Acentos;
        }

    }
}

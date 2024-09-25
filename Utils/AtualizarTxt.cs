using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace TestePortal.Utils
{
    public class AtualizarTxt
    {
        public static async Task<string> AtualizarDataEEnviarArquivo(IPage page, string caminhoArquivo)
        {
            var linhas = File.ReadAllLines(caminhoArquivo);

            // Atualizando data
            string dataAtual = DateTime.Now.ToString("ddMMyy");
            string DataArquivoTemplate = linhas[0].Substring(94, 6);
            string AnteriorData = linhas[0].Substring(0, 94);
            string PosData = linhas[0].Substring(101);
            linhas[0] = linhas[0].Replace("#DATA#", dataAtual);

            //atualizando num consultoria
            Random random = new Random();
            for (int i = 1; i <= 7; i++)
            {
                // números aleatórios para substituir o marcador #DOC_NUMERO_CONSULTORIA#
                string randomNumber = "";
                for (int j = 0; j < 25; j++) 
                {
                    randomNumber += random.Next(0, 10).ToString();
                }

                // Substitui o marcador #DOC_NUMERO_CONSULTORIA# 
                linhas[i] = linhas[i].Replace("#DOC_NUMERO_CONSULTORIA_#", randomNumber);

                string randomNumberNumDoc = "";
                for (int j = 0; j < 10; j++) 
                {
                    randomNumberNumDoc += random.Next(0, 10).ToString();
                }

                // #NUM_DOCU# nas colunas 112 a 121 com a string gerada
                linhas[i] = linhas[i].Replace("#NUM_DOCU#", randomNumberNumDoc);
            }


            
            string dataFormatada = DateTime.Now.ToString("yyyyMMdd");
            int indice = 1;
            string novoNomeArquivo;
            string novoCaminhoArquivo;

            do
            {
                novoNomeArquivo = $"FundoQA_{dataFormatada}.{indice}.txt";
                novoCaminhoArquivo = Path.Combine(Path.GetDirectoryName(caminhoArquivo), novoNomeArquivo);
                indice++;
            } while (File.Exists(novoCaminhoArquivo));

            File.WriteAllLines(novoCaminhoArquivo, linhas);

            await page.Locator("#fileEnviarOperacoes").SetInputFilesAsync(new[] { novoCaminhoArquivo });

            Console.WriteLine($"Arquivo {novoNomeArquivo} enviado com sucesso.");

            return novoNomeArquivo;
        }
    }

}
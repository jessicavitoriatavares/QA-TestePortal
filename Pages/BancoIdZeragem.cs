using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class BancoIdZeragem
    {
        public static async Task<Model.Pagina> Zeragem (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {

                var BancoIdZeragem = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/BancoID/Zeragem.aspx");

                if (BancoIdZeragem.Status == 200)
                {
                    Console.Write("Zeragem - Banco ID: ");
                    Console.WriteLine(BancoIdZeragem.Status);
                    pagina.StatusCode = BancoIdZeragem.Status;
                    pagina.Nome = "Zeragem";
                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Zeragem no tópico Banco ID ");
                    pagina.Nome = "Zeragem";
                    pagina.StatusCode = BancoIdZeragem.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }


            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}

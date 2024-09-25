using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class CedentesKitCedente
    {
        public static async Task<Model.Pagina> KitCedentes(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Page.WaitForLoadStateAsync();
            await Page.WaitForTimeoutAsync(1000);

            try
            {
                var BoletagemKitCedentes = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/KitCedente.aspx");

                if (BoletagemKitCedentes.Status == 200)
                {
                    Console.Write("Kit Cedentes - Boletagem : ");
                    pagina.Nome = "Kit Cedentes";
                    pagina.StatusCode = BoletagemKitCedentes.Status;
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
                    Console.Write("Erro ao carregar a página de Kit Cedentes no tópico Boletagem ");
                    pagina.Nome = "Kit Cedentes";
                    pagina.StatusCode = BoletagemKitCedentes.Status;
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

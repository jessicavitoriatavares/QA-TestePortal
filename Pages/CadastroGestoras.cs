using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class CadastroGestoras
    {

        public static async Task<Model.Pagina> Gestoras (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var CadastroGestora = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Gestoras.aspx");

                if (CadastroGestora.Status == 200)
                {
                    Console.Write("Gestoras  - Cadastro: ");
                    pagina.Nome = "Gestoras";
                    pagina.StatusCode = CadastroGestora.Status;
                    pagina.Listagem = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")

                    {
                        errosTotais++;
                    
                    }
                    pagina.BaixarExcel = "❓";
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Gestoras no tópico Cadastro ");
                    pagina.Nome = "Gestoras";      
                    pagina.StatusCode = CadastroGestora.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.InserirDados = "❌";
                pagina.Excluir = "❌";
                errosTotais += 2;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}

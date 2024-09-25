using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class RelatoriosOperacoes
    {
        public static async Task<Model.Pagina> Operacoes (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var RelatorioOperacoes = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Relatorios/Operacoes.aspx");

                if (RelatorioOperacoes.Status == 200)
                {
                    Console.Write("Operações - Relatorios : ");
                    pagina.Nome = "Operações - Relatorios";                  
                    pagina.StatusCode = RelatorioOperacoes.Status;
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
                    Console.Write("Erro ao carregar o relatório de Operações: ");
                    pagina.Nome = "Operações - Relatorios";
                    pagina.StatusCode = RelatorioOperacoes.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                pagina.TotalErros = errosTotais ;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }


    }
}

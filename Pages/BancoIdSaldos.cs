using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class BancoIdSaldos
    {
        public static async Task<Model.Pagina> Saldos (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var BancoIdSaldos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/BancoID/Saldos.aspx");


                if (BancoIdSaldos.Status == 200)
                {
                    Console.Write("Saldos - Banco ID: ");
                    Console.WriteLine(BancoIdSaldos.Status);
                  //  pagina.ListaErros = listErros;
                    pagina.StatusCode = BancoIdSaldos.Status;
                    pagina.Nome = "Saldos";
                    listErros.Add("0");
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
                    Console.Write("Erro ao carregar a página de saldos no tópico Banco ID ");
                    Console.WriteLine(BancoIdSaldos.Status);
                    listErros.Add("Erro ao carregar a página de saldos no tópico Banco ID ");
                    pagina.Nome = "Saldos";
                   pagina.StatusCode = BancoIdSaldos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

    }
}

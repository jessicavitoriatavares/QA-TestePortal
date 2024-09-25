using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class BancoIdExtratos
    {
        public static async Task<Model.Pagina> Extratos (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var BancoIdExtratos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Relatorios/BancoID.aspx");

                if (BancoIdExtratos.Status == 200)
                {
                    Console.Write("Extratos - Banco ID: ");
                    Console.WriteLine(BancoIdExtratos.Status);
                   // pagina.ListaErros = listErros;
                    pagina.StatusCode = BancoIdExtratos.Status;
                    pagina.Nome = "Extratos";
                    listErros.Add("0");
                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓    ";
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
                    Console.Write("Erro ao carregar a página de Devolução/Reembolso no tópico Banco ID ");
                    Console.WriteLine(BancoIdExtratos.Status);
                    listErros.Add("Erro ao carregar a página de Devolução/Reembolso no tópico Banco ID ");
                    pagina.Nome = "Extratos";
                    pagina.StatusCode = BancoIdExtratos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                   
                }

            }
            catch (TimeoutException ex) {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

    }
}

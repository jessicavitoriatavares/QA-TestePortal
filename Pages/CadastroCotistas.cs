using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class CadastroCotistas
    {

        public static async Task<Model.Pagina> Cotista (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var CadastroCotista = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Cotistas.aspx");

                if (CadastroCotista.Status == 200)
                {
                    Console.Write("Cotista - Cadastro: ");
                    Console.WriteLine(CadastroCotista.Status);
                //    pagina.ListaErros = listErros;
                    pagina.StatusCode = CadastroCotista.Status;
                    pagina.Nome = "Cotista";
                    listErros.Add("0");
                    pagina.Listagem = "❓ ";
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
                    Console.Write("Erro ao carregar a página de Cotista no tópico Cadastro ");
                    pagina.Nome = "Cotista";
                    pagina.StatusCode = CadastroCotista.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais+=2;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }  
    }  

}

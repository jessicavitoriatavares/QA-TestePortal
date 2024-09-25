using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class CadastroCarteira
    {
        public static async Task<Model.Pagina> Carteira (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {

                var CadastroCarteira = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Carteira/Carteiras.aspx");

                if (CadastroCarteira.Status == 200)
                {
                    string seletorTabela = "#tabelaCarteiras";

                    Console.Write("Carteira - Cadastro: ");
                    pagina.Nome = "Carteira";
                    pagina.StatusCode = CadastroCarteira.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Carteira no tópico Cadastro ");
                    Console.WriteLine(CadastroCarteira.Status);
                    listErros.Add("Erro ao carregar a página de Carteira no tópico Cadastro ");
                    pagina.Nome = "Carteira";
                    pagina.StatusCode = CadastroCarteira.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");

                }
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
        }

    }
}

using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestePortal.Pages
{
    public class CedentesCedentes
    {
        public static async Task<Model.Pagina> Cedentes(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            await Page.WaitForLoadStateAsync();

            try
            {
                var BoletagemCedentes = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Cedentes.aspx");

                if (BoletagemCedentes.Status == 200)
                {
                    string seletorTabela = "#tabelaCedentes";

                    Console.Write("Cedentes: ");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = BoletagemCedentes.Status;
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
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "36614123000160" });
                    await Page.Locator("#fileNovoCedente").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "36614123000160_53300608000106_N.zip" });
                    var cedenteCadastrado = await Page.WaitForSelectorAsync("text=Cedente Cadastrado com Sucesso", new PageWaitForSelectorOptions

                    {

                    Timeout = 20000
                   
                    });

                    if (cedenteCadastrado != null)
                    {
                        var cedenteExiste = Repository.Cedentes.CedentesRepository.VerificaExistenciaCedente("36614123000160", "53300608000106");
                        var apagarCedente = Repository.Cedentes.CedentesRepository.ApagarCedente("36614123000160", "53300608000106");

                        if (cedenteExiste)
                        {
                            Console.WriteLine("Cedente adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            if (apagarCedente)
                            {
                                Console.WriteLine("Cedente apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar cedente");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir cedente");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }
                    }
                    else
                    {
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }
                }


                else
                {
                    Console.Write("Erro ao carregar a página de Cedentes no tópico Boletagem ");
                    pagina.Nome = "Cedentes";
                    pagina.StatusCode = BoletagemCedentes.Status;
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
                //await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }

    }
}

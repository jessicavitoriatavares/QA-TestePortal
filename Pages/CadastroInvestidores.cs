using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestePortal.Pages
{
    public class CadastroInvestidores
    {
        public static async Task<Model.Pagina> Investidores (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var CadastroInvestidores = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/CotistaInterno.aspx");
                if (CadastroInvestidores.Status == 200)
                {
                    string seletorTabela = "#tabelaCotista";

                    Console.Write("Investidores - Cadastro: ");
                    Console.WriteLine(CadastroInvestidores.Status);
                    //pagina.ListaErros = listErros;
                    pagina.StatusCode = CadastroInvestidores.Status;
                    pagina.Nome = "Investidores";
                    listErros.Add("0");
                    pagina.BaixarExcel = "❓";
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

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#cpfCnpjCotistaInterno").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#cpfCnpjCotistaInterno").FillAsync("49624866830");
                    await Task.Delay(300);
                    await Page.Locator("#btnAvancarCadastroCotista").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#emailCotistaInterno").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#emailCotistaInterno").FillAsync("jessica.tavares@gmail.com");
                    await Task.Delay(300);
                    await Page.Locator("#obsCotistaInterno").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#obsCotistaInterno").FillAsync("investidora ");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

                    var investidorExiste = Repository.Investidores.InvestidoresRepository.VerificaExistenciaInvestidores("49624866830", "jessica.tavares@gmail.com");

                    if (investidorExiste)
                    {
                        Console.WriteLine("investidor adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarInvestidor = Repository.Investidores.InvestidoresRepository.ApagarInvestidores("49624866830", "jessica.tavares@gmail.com");

                        if (apagarInvestidor)
                        {
                            Console.WriteLine("Investidor apagado com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Investidor");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir investidor");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Investidores no tópico Cadastro ");
                    pagina.Nome = "Investidores";             
                    pagina.StatusCode = CadastroInvestidores.Status;
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

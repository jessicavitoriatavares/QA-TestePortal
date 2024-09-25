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
    public class BoletagemAmortizacao
    {
        public static async Task<Model.Pagina> Amortizacao (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Task.Delay(500);

            try
            {

                var BoletagemAmortizacao = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Boleta/Amortizacao.aspx");

                if (BoletagemAmortizacao.Status == 200)
                {
                    string seletorTabela = "#tabelaAmortizacao";

                    Console.Write("Amortizacao - Boletagem : ");
                    pagina.Nome = "Amortizacao";
                    pagina.StatusCode = BoletagemAmortizacao.Status;
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

                    await Page.GetByRole(AriaRole.Link, new() { Name = " Boletagem " }).ClickAsync();
                    await Task.Delay(200);
                    await Page.GetByRole(AriaRole.Link, new() { Name = " Amortização" }).ClickAsync();
                    await Task.Delay(200);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                    await Task.Delay(200);
                    await Page.Locator("#fundoAmortizacao").SelectOptionAsync(new[] { "36614123000160" });
                    await Task.Delay(200);
                    await Page.GetByPlaceholder("/00/0000").ClickAsync();
                    await Task.Delay(200);
                    await Page.GetByPlaceholder("/00/0000").FillAsync("05/09/2024");
                    await Task.Delay(200);
                    await Page.Locator("#valorTotalAmortizacao").FillAsync("10");
                    await Task.Delay(200);
                    await Page.GetByLabel("Nome do Cotista:*").ClickAsync();
                    await Task.Delay(200);
                    await Page.GetByLabel("Nome do Cotista:*").FillAsync("Jessica Vitoria Tavares");
                    await Task.Delay(200);
                    await Page.GetByLabel("Cpf / Cnpj do Cotista:*").ClickAsync();
                    await Task.Delay(200);
                    await Page.GetByLabel("Cpf / Cnpj do Cotista:*").FillAsync("49624866830");
                    await Task.Delay(200);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
                    await Task.Delay(500);

                    var BoletagemAmortizacaoExiste = Repository.BoletagemAmortizacao.BoletagemAmortizacaoRepository.VerificaExistenciaBoletagemAmortizacao("Jessica Vitoria Tavares", "49624866830");
                    

                    if (BoletagemAmortizacaoExiste)
                    {
                        Console.WriteLine("Amortização adicionada com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarBoletagemAmortizacao = Repository.BoletagemAmortizacao.BoletagemAmortizacaoRepository.ApagarBoletagemAmortizacao("Jessica Vitoria Tavares", "49624866830");

                        if (apagarBoletagemAmortizacao)
                        {
                            Console.WriteLine("Amortização apagada com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Amortização");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Amortização");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Amortizacao no tópico Boletagem ");
                    pagina.Nome = "Amortizacao";
                    pagina.StatusCode = BoletagemAmortizacao.Status;
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

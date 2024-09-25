using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestePortal.Model;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages
{
    public class BancoIdContasEscrow
    {

        public static async Task<Model.Pagina> ContasEscrow (IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {

                var PaginaContasEscrow = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Escrow/Escrows.aspx");
                if (PaginaContasEscrow.Status == 200)
                {

                    Console.Write("Banco Id - Contas Escrow: ");
                    pagina.StatusCode = PaginaContasEscrow.Status;
                    pagina.Nome = "Contas Escrow";
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Excluir = "❌";
                    pagina.InserirDados = "❌";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    string seletorTabela = "#tabelaContasEscrows";
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora)
                    {

                        await Page.GetByRole(AriaRole.Button, new() { Name = " Nova" }).ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#fundoBanco").SelectOptionAsync(new[] { "36614123000160" });
                        await Task.Delay(50);
                        await Page.Locator("#contratoBanco").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#contratoBanco").FillAsync("contrato teste");
                        await Task.Delay(50);
                        await Page.Locator("#titularBanco").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#titularBanco").FillAsync("Jessica Vitoria Tavares  ");
                        await Task.Delay(50);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/0000-00" }).ClickAsync();
                        await Task.Delay(50);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/0000-00" }).FillAsync("53300608000106");
                        await Task.Delay(50);
                        await Page.Locator("#titularDestino").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#titularDestino").FillAsync("Jessica Vitoria Tavares ");
                        await Task.Delay(50);
                        await Page.Locator("#cnpjDestino").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#cnpjDestino").FillAsync("49624866830");
                        await Task.Delay(50);
                        await Page.Locator("#bancoDestino").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#bancoDestino").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#bancoDestino").FillAsync("439");
                        await Task.Delay(50);
                        await Page.Locator("#agenciaDestino").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#agenciaDestino").FillAsync("0001");
                        await Task.Delay(50);
                        await Page.Locator("#contaDestino").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#contaDestino").FillAsync("46091");
                        await Task.Delay(50);
                        await Page.Locator("#digcontaDestino").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#digcontaDestino").FillAsync("5");
                        await Task.Delay(50);
                        await Page.Locator("#valorDestino").ClickAsync();
                        await Task.Delay(50);
                        await Page.Locator("#valorDestino").FillAsync("R$10");
                        await Task.Delay(50);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        var contasEcrowExiste = Repository.ContasEscrows.ContasEscrows.VerificaExistenciaContasEscrow("adm@idsf.com.br", "Jessica Vitoria Tavares");
                       

                        if (contasEcrowExiste)
                        {
                            Console.WriteLine("Conta Escrow adicionada com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            var apagarContasEscrow = Repository.ContasEscrows.ContasEscrows.ApagarContasEscrow("adm@idsf.com.br", "Jessica Vitoria Tavares");
                            if (apagarContasEscrow)
                            {
                                Console.WriteLine("Conta Escrow apagada com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Conta Escrow");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Conta Escrow");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }

                    }
                    else {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Contas Escrow no tópico Banco ID ");
                    Console.WriteLine(PaginaContasEscrow.Status);
                    pagina.Nome = "Contas Escrow";                 
                    pagina.StatusCode = PaginaContasEscrow.Status;
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

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
    public class NotaComercial
    {
        public static async Task<Model.Pagina> NotasComerciais(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var NotaComercial = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Operacoes/NotaComercial.aspx");

                if (NotaComercial.Status == 200)
                {
                    string seletorTabela = "#tabelaNotaComercial";

                    try
                    {
                        Console.Write("Nota Comercial: ");
                        Console.WriteLine(NotaComercial.Status);
                     
                        pagina.StatusCode = NotaComercial.Status;
                        pagina.Nome = "Notas Comerciais";
                        listErros.Add("0");
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

                        await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#Fundos").SelectOptionAsync(new[] { "36614123000160" });
                        await Task.Delay(200);
                        await Page.Locator("#Produtos").SelectOptionAsync(new[] { "35" });
                        await Task.Delay(200);
                        await Page.Locator("#tomador").SelectOptionAsync(new[] { "501" });
                        await Task.Delay(200);
                        await Page.Locator("#tipo").SelectOptionAsync(new[] { "pix" });
                        await Task.Delay(200);
                        await Page.Locator("#contaLiquidacao").SelectOptionAsync(new[] { "702" });
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste jessica");
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Tab, new() { Name = "Envolvidos" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "  Adicionar envolvido" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#relacionado").SelectOptionAsync(new[] { "501" });
                        await Task.Delay(200);
                        await Page.Locator("#envolvido").SelectOptionAsync(new[] { "42679242874" });
                        await Task.Delay(200);
                        await Page.Locator("#tipoRelacao").SelectOptionAsync(new[] { "empregador" });
                        await Task.Delay(200);
                        await Page.ClickAsync("button[onclick='adicionarEnvolvido()']");
                        await Task.Delay(500);
                        await Page.ClickAsync("button[onclick=\"abrirModalAddEnvolvido('novo')\"]");
                        await Task.Delay(200);
                        await Page.Locator("#relacionado").SelectOptionAsync(new[] { "idsf" });
                        await Task.Delay(200);
                        await Page.Locator("#envolvido").SelectOptionAsync(new[] { "46837686828" });
                        await Task.Delay(200);
                        await Page.Locator("#tipoRelacao").SelectOptionAsync(new[] { "cedente" });
                        await Task.Delay(200);
                        await Page.ClickAsync("button[onclick='adicionarEnvolvido()']");
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Tab, new() { Name = "Operação" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.FillAsync("#valorSocilicitado", "10000");
                        await Task.Delay(200);
                        await Page.FillAsync("#taxaJuros", "1,20");
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Duração:*" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Duração:*" }).FillAsync("10");
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Carência Amortização:*" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#tipoCalculo").SelectOptionAsync(new[] { "bruto" });
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Dia de Vencimento:*" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Spinbutton, new() { Name = "Dia de Vencimento:*" }).FillAsync("1");
                        await Task.Delay(200);
                        string dataAtual = DateTime.Now.ToString("dd/MM/yyyy");
                        await Page.FillAsync("#dataInicio", dataAtual);
                        await Page.FillAsync("#corban", "1");
                        await Task.Delay(200);
                        await Page.ClickAsync("#btnSalvarMudancas");
                        await Task.Delay(400);


                        var notaComercialExiste = Repository.NotaComercial.NotaComercialRepository.VerificaExistenciaNotaComercial("36614123000160", "teste jessica");
                      

                        if (notaComercialExiste)
                        {
                            Console.WriteLine("Nota comercial adicionada com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarNotaComercial = Repository.NotaComercial.NotaComercialRepository.ApagarNotaComercial("36614123000160", "teste jessica");

                            if (apagarNotaComercial)
                            {
                                Console.WriteLine("Nota comercial apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Nota comercial");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Nota comercial");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }

                    }
                    catch (TimeoutException ex)
                    {
                        Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                        Console.WriteLine($"Exceção: {ex.Message}");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais++;
                        await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/login.aspx");

                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Notas Comercial ");
                    pagina.Nome = "Notas Comerciais";
                    pagina.StatusCode = NotaComercial.Status;
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
                errosTotais+=2;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}

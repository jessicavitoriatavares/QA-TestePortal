using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages
{
    public class BoletagemResgate
    {
        public static async Task<Model.Pagina> Resgate (IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var BoletagemResgate = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Boleta/Resgate.aspx");

                if (BoletagemResgate.Status == 200)
                {
                    string seletorTabela = "#tabelaResgates";

                    Console.Write("Resgate - Boletagem : ");
                    pagina.Nome = "Resgate";
                    pagina.StatusCode = BoletagemResgate.Status;
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

                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                        await Page.Locator("#dataCorteInput").ClickAsync();
                        await Page.Locator("#dataCorteInput").FillAsync("30/08/2024");
                        await Page.Locator("#dataLiquidInput").ClickAsync();
                        await Page.Locator("#dataLiquidInput").FillAsync("30/08/2024");
                        await Page.Locator("#NomeCotista").ClickAsync();
                        await Page.Locator("#NomeCotista").FillAsync("Jessica Vitoria Tavares");
                        await Page.Locator("#CpfCnpjCotista").ClickAsync();
                        await Page.Locator("#CpfCnpjCotista").FillAsync("49624866830");
                        await Page.GetByLabel("Valor Bruto:").ClickAsync();
                        await Page.GetByLabel("Valor Bruto:").FillAsync("10");
                        await Page.GetByLabel("Valor Liquido:").FillAsync("R$11");
                        await Page.GetByLabel("Valor IR:").ClickAsync();
                        await Page.GetByLabel("Valor IR:").FillAsync("1");
                        await Page.GetByLabel("Fundo:*").SelectOptionAsync(new[] { "36614123000160" });
                        await Page.Locator("#CodBanco").ClickAsync();
                        await Page.Locator("#CodBanco").FillAsync("439");
                        await Page.Locator("#Agencia").ClickAsync();
                        await Page.Locator("#Agencia").FillAsync("0001");
                        await Page.Locator("#ContaCorrente").ClickAsync();
                        await Page.Locator("#ContaCorrente").FillAsync("46091");
                        await Page.Locator("#DigitoConta").ClickAsync();
                        await Page.Locator("#DigitoConta").FillAsync("5");
                        await Page.Locator("#fileResgate").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "documentosteste.zip" });
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("obs");
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        var BoletagemResgateExiste = Repository.BoletagemResgate.BoletagemResgateRepository.VerificaExistenciaBoletagemResgate("49624866830", "36614123000160");

                        if (BoletagemResgateExiste)
                        {
                            Console.WriteLine("Resgate adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarBoletagemResgate = Repository.BoletagemResgate.BoletagemResgateRepository.ApagarBoletagemResgate("49624866830", "36614123000160");

                            if (apagarBoletagemResgate)
                            {
                                Console.WriteLine("Resgate apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Resgate");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Resgate");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                        }

                    }
                    else
                    {
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";

                    } 

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Resgate no tópico Boletagem ");
                    pagina.Nome = "Resgate";
                    pagina.StatusCode = BoletagemResgate.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (Exception ex)
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

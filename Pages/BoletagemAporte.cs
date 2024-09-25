using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages
{
    public class BoletagemAporte
    {
        public static async Task<Model.Pagina> Aporte (IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var BoletagemAporte = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Boleta/Boleta.aspx");

                if (BoletagemAporte.Status == 200)
                {
                    string seletorTabela = "#tabelaBoletas";
                    // string seletorBotao = "#exportarButton";

                    Console.Write("Aporte - Boletagem : ");
                    pagina.Nome = "Aporte";
                    pagina.StatusCode = BoletagemAporte.Status;
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
                    pagina.BaixarExcel = "❌";

                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }// botão duplicado na página.  //exportarButton

                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora || nivelLogado == NivelEnum.Consultoria)
                    {

                        await Page.Locator("#tableButton").ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).FillAsync("30/08/2024");
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "0000,00" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "0000,00" }).FillAsync("R$10");
                        await Task.Delay(200);
                        await Page.Locator("#CPFCotista").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#CPFCotista").FillAsync("49624866830");
                        await Task.Delay(200);
                        await Page.Locator("#NomeCotista").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#NomeCotista").FillAsync("Jessica Tavares");
                        await Task.Delay(200);
                        await Page.GetByPlaceholder("Tipo de Cota").ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByPlaceholder("Tipo de Cota").FillAsync("cota");
                        await Task.Delay(200);
                        await Page.GetByText("Escolha o fundo:* Escolha o").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#Fundos").SelectOptionAsync(new[] { "36614123000160" });
                        await Task.Delay(200);
                        await Page.Locator("#fileBoleta").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "documentosteste.zip" });
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("Observaç");
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        var BoletagemAporteExiste = Repository.BoletagemAporte.BoletagemAporteRepository.VerificaExistenciaBoletagemAporte("Jessica Tavares", "cota");

                        if (BoletagemAporteExiste)
                        {
                            Console.WriteLine("Boleta adicionada com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarBoletagemAporte = Repository.BoletagemAporte.BoletagemAporteRepository.ApagarBoletagemAporte("Jessica Tavares", "cota");

                            if (apagarBoletagemAporte)
                            {
                                Console.WriteLine("Boleta apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Boleta");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Boleta");
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
                    Console.Write("Erro ao carregar a página de Aporte no tópico Boletagem ");
                    pagina.Nome = "Aporte";
                    pagina.StatusCode = BoletagemAporte.Status;
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

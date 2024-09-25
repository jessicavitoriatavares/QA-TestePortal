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
    public class NotasPagamentos
    {
        public static async Task<Model.Pagina> Pagamentos(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            await Page.WaitForLoadStateAsync();
            await Page.WaitForLoadStateAsync();
            await Page.WaitForTimeoutAsync(1000);

            try
            {

                var NotasPagamentos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Notas/PagamentosNotas.aspx");
                if (NotasPagamentos.Status == 200)
                {
                    string seletorTabela = "#tabelaNotas";

                    Console.Write("Pagamentos - Notas : ");
                    pagina.Nome = "Pagamentos";
                    pagina.StatusCode = NotasPagamentos.Status;
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
                        await Task.Delay(300);
                        await Page.Locator("#agendamentoFiltro").FillAsync("2024-09-02");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "/00/0000" }).FillAsync("02/12/2024");
                        await Task.Delay(300);
                        await Page.Locator("#tipoNota").SelectOptionAsync(new[] { "ASSEMBLEIA" });
                        await Task.Delay(300);
                        await Page.GetByPlaceholder("0000,00").ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByPlaceholder("0000,00").FillAsync("1");
                        await Task.Delay(300);
                        await Page.Locator("#Fundos").SelectOptionAsync(new[] { "36614123000160" });
                        await Task.Delay(300);
                        await Page.Locator("#Prestadores").SelectOptionAsync(new[] { "276" });
                        await Task.Delay(300);
                        await Page.Locator("#filePagamentosNotas").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "21321321321.pdf" });
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste jessica");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        var notaPagamentoExiste = Repository.NotaPagamento.NotaPagamentoRepository.VerificaExistenciaNotaPagamento("36614123000160", "teste jessica");

                        if (notaPagamentoExiste)
                        {
                            Console.WriteLine("Notas pagamento adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarNotaPagamento = Repository.NotaPagamento.NotaPagamentoRepository.ApagarNotaPagamento("36614123000160", "teste jessica");

                            if (apagarNotaPagamento)
                            {
                                Console.WriteLine("Notas pagamento apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar pagamento nota");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir pagamento nota");
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
                    Console.Write("Erro ao carregar a página de Pagamentos no tópico Notas ");
                    pagina.Nome = "Pagamentos";
                    pagina.StatusCode = NotasPagamentos.Status;
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

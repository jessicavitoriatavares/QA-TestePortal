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
    public class BancoIdReembolso
    {

        public static async Task<Model.Pagina> Reembolso(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {

                var BancoIdDevolucao = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/BancoID/Reembolso.aspx");

                if (BancoIdDevolucao.Status == 200)
                {
                    Console.Write("Devolução/Reembolso - Banco ID: ");
                    pagina.StatusCode = BancoIdDevolucao.Status;
                    pagina.Nome = "Devolução/Reembolso";
                    listErros.Add("0");
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    string seletorTabela = "#tabelaReembolso";
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }


                    if (nivelLogado == NivelEnum.Master || nivelLogado == NivelEnum.Gestora)
                    {
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                        await Task.Delay(100);
                        await Page.Locator("#fundoBanco").SelectOptionAsync(new[] { "36614123000160" });
                        await Task.Delay(200);
                        await Page.Locator("#titularBanco").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#titularBanco").FillAsync("Jessica Vitoria Tavares");
                        await Task.Delay(200);
                        await Page.Locator("#numeroDocumento").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#numeroDocumento").FillAsync("599997655");
                        await Task.Delay(200);
                        await Page.Locator("#cnpjBanco").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#cnpjBanco").FillAsync("53300608000106");
                        await Task.Delay(200);
                        await Page.Locator("#codBancoOrigem").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#codBancoOrigem").FillAsync("439");
                        await Task.Delay(200);
                        await Page.Locator("#contaBanco").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#contaBanco").FillAsync("46121");
                        await Task.Delay(200);
                        await Page.Locator("#digcontaBanco").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#digcontaBanco").FillAsync("0");
                        await Task.Delay(200);
                        await Page.Locator("#titularDestino").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#titularDestino").FillAsync("Jessica Vitoria Tavares");
                        await Task.Delay(200);
                        await Page.Locator("#cnpjDestino").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#cnpjDestino").FillAsync("45543915000181");
                        await Task.Delay(200);
                        await Page.Locator("#bancoDestino").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#bancoDestino").FillAsync("439");
                        await Task.Delay(200);
                        await Page.Locator("#agenciaDestino").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#agenciaDestino").FillAsync("0001");
                        await Task.Delay(200);
                        await Page.Locator("#contaDestino").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#contaDestino").FillAsync("46091");
                        await Task.Delay(200);
                        await Page.Locator("#digcontaDestino").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#digcontaDestino").FillAsync("5");
                        await Task.Delay(200);
                        await Page.Locator("#valorDestino").ClickAsync();
                        await Task.Delay(200);
                        await Page.Locator("#valorDestino").FillAsync("R$10");
                        await Task.Delay(200);
                        await Page.Locator("#tipo").SelectOptionAsync(new[] { "reembolso" });
                        await Task.Delay(200);
                        await Page.GetByPlaceholder("Insira a descrição").ClickAsync();
                        await Task.Delay(200);
                        await Page.GetByPlaceholder("Insira a descrição").FillAsync("Reembolso do valor ");
                        await Task.Delay(200);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Enviar" }).ClickAsync();

                        var ReembolsoExiste = Repository.Reembolso.ReembolsoRepository.VerificaExistenciaReembolso("599997655", "Jessica Vitoria Tavares");
                        var apagarReembolso = Repository.Reembolso.ReembolsoRepository.ApagarReembolso("599997655", "Jessica Vitoria Tavares");

                        if (ReembolsoExiste)
                        {
                            Console.WriteLine("Reembolso adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";

                            if (apagarReembolso)
                            {
                                Console.WriteLine("Reembolso apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Reembolso");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir Reembolso");
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
                    Console.Write("Erro ao carregar a página de Devolução/Reembolso no tópico Banco ID ");
                    Console.WriteLine(BancoIdDevolucao.Status);
                    listErros.Add("Erro ao carregar a página de Devolução/Reembolso no tópico Banco ID ");
                    pagina.Nome = "Devolução/Reembolso";
                    errosTotais++;
                    pagina.StatusCode = BancoIdDevolucao.Status;
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

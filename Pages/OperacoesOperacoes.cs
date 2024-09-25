using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestePortal.Utils;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading;

namespace TestePortal.Pages
{
    public class OperacoesOperacoes
    {
        public static async Task<Model.Pagina> Operacoes(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try

            {
                var OperacoesOperacoes = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Operacoes/Operacoes.aspx");

                if (OperacoesOperacoes.Status == 200)
                {
                    string seletorTabela = "#divTabelaCedentes";

                    Console.Write("Operações - Operações : ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesOperacoes.Status;
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

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Nova Operação +" }).ClickAsync();
                    //await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "53300608000106" });
                    await Page.Locator("#selectFundo").SelectOptionAsync(new[] { "36614123000160" });

                    //Atualizar TXT
                    string caminhoArquivo = @"C:\Temp\Arquivos\CNABz.txt";
                    string novoNomeArquivo = await AtualizarTxt.AtualizarDataEEnviarArquivo(Page, caminhoArquivo);
                    await Task.Delay(500);
                    var CadastroOperacoes = await Page.GetByText("Arquivo recebido com sucesso! Aguarde a Validação").ElementHandleAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Close" }).ClickAsync();
                    await Task.Delay(20000);

                    if (CadastroOperacoes != null)
                    {
                        var operacoesExiste = Repository.Operacoes.OperacoesRepository.VerificaExistenciaOperacao(novoNomeArquivo);

                        var operacoesRepository = new Repository.Operacoes.OperacoesRepository();
                        await Task.Delay(100);

                        if (operacoesExiste)
                        {
                            var apagarOperacao = await operacoesRepository.ExcluirOperacaoAsync(Page, novoNomeArquivo);
                            Console.WriteLine("Operação lançada.");
                            pagina.InserirDados = "✅";

                            if (apagarOperacao)
                            {
                                Console.WriteLine("Operação apagada com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar operação");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível lançar operação");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;

                        }


                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Operações no tópico Operações: ");
                    pagina.Nome = "Operações - Operações";
                    pagina.StatusCode = OperacoesOperacoes.Status;
                    errosTotais += 2;
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

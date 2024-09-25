using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static TestePortal.Model.Usuario;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages
{
    public class OperacoesAtivos
    {
        public static async Task<Model.Pagina> Ativos (IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {

                var OperacoesAtivos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Operacoes/Contratos.aspx");


                if (OperacoesAtivos.Status == 200)
                {
                    string seletorTabela = "#tabelaContratos";
                    Console.Write("Operações - Ativos : ");
                    pagina.Nome = "Operações - Ativos";
                    pagina.StatusCode = OperacoesAtivos.Status;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
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
                        await Page.GetByRole(AriaRole.Button, new() { Name = "+ Novo" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#Fundos").SelectOptionAsync(new[] { "36614123000160" });
                        await Task.Delay(300);
                        await Page.FillAsync("#nomeCedente", "cedente");
                        await Task.Delay(300);
                        await Page.FillAsync("#cpfCnpjCedente", "533.006.080-00106");
                        await Task.Delay(300);
                        await Page.Locator("#tipoNota").SelectOptionAsync(new[] { "AtivosImobiliarios" });
                        await Task.Delay(300);
                        await Page.Locator("#AgenciaAtivos").FillAsync("0001");
                        await Task.Delay(300);
                        await Page.Locator("#ContaAtivos").FillAsync("460915");
                        await Task.Delay(300);
                        await Page.Locator("#RazSocDestino").FillAsync("razão");
                        await Task.Delay(300);
                        await Page.Locator("#CpfCnpjAtivos").FillAsync("49624866830");
                        await Page.GetByLabel("Valor").ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByLabel("Valor").FillAsync("10");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Insira a mensagem" }).FillAsync("teste jessica");
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Anexos" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Anterior" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("input[data-id-anexo='7']").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "21321321321.pdf" });
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Voltar" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();


                        var ativoExiste = Repository.Ativos.AtivosRepository.VerificaExistenciaAtivos("36614123000160", "teste jessica");

                        if (ativoExiste)
                        {
                            Console.WriteLine("Ativo adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarAtivo = Repository.Ativos.AtivosRepository.ApagarAtivos("36614123000160", "teste jessica");
                            if (apagarAtivo)
                            {
                                Console.WriteLine("Ativo apagado com sucesso");
                                pagina.Excluir = "✅";
                                await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");

                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar ativo ");

                                pagina.Excluir = "❌";
                                errosTotais++;
                                await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                            }


                        }


                        else
                        {
                            Console.WriteLine("Não foi possível inserir ativo");
                            pagina.InserirDados = "❌";
                            pagina.Excluir = "❌";
                            errosTotais += 2;
                            await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
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
                    Console.Write("Erro ao carregar a página de Ativos de baixa no tópico Operações ");
                    pagina.Nome = "Operações - Ativos";
                    pagina.StatusCode = OperacoesAtivos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex) {
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

using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.IO;
using System.Configuration;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages
{
    public class CadastroFundosTransferencia
    {

        public static async Task<Model.Pagina> FundosTransf (IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var CadastroFundosTransferencia = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/FundosTransferencia.aspx");

                if (CadastroFundosTransferencia.Status == 200)
                {
                    string seletorTabela = "#tabelaUsuarios";

                    Console.Write("Fundos De Transferencia - Cadastro: ");
                    pagina.Nome = "Fundos De Transferencia";
                    pagina.StatusCode = CadastroFundosTransferencia.Status;
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
                    if (nivelLogado == NivelEnum.Master)

                    {

                        await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                        await Page.Locator("#NomeFundo").ClickAsync();
                        await Page.Locator("#NomeFundo").FillAsync("QA teste");
                        await Page.GetByPlaceholder("/0000-00").ClickAsync();
                        await Page.GetByPlaceholder("/0000-00").FillAsync("45.543.915/0001-81");
                        await Page.Locator("#Gestora").SelectOptionAsync(new[] { "16007398000128" });
                        await Page.Locator("#CoGestora").SelectOptionAsync(new[] { "21046086000163" });
                        await Page.Locator("#Consultora").SelectOptionAsync(new[] { "11578970000195" });
                        await Page.Locator("#CoConsultora").SelectOptionAsync(new[] { "26452257000178" });
                        await Page.Locator("#antigoAdministrador").ClickAsync();
                        await Page.Locator("#antigoAdministrador").FillAsync("Administrador");
                        await Page.Locator("#tipoFundo").SelectOptionAsync(new[] { "FIDC" });
                        await Page.Locator("#tipoInvestidor").SelectOptionAsync(new[] { "PROFISSIONAL" });
                        await Page.Locator("#mercadoFundo").SelectOptionAsync(new[] { "ABERTURA" });
                        await Page.Locator("#agenteCobranca").ClickAsync();
                        await Page.Locator("#agenteCobranca").FillAsync("Cobrança");
                        await Page.Locator("#antigaGestora").ClickAsync();
                        await Page.Locator("#antigaGestora").FillAsync("antiga");
                        await Page.Locator("#antigaConsultoria").ClickAsync();
                        await Page.Locator("#antigaConsultoria").FillAsync("Consultoria");
                        await Page.Locator("#FileArchives").SetInputFilesAsync(new[] { ConfigurationManager.AppSettings["PATH.ARQUIVO"].ToString() + "documentosteste.zip" });
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                        var fundoTransferenciaExiste = Repository.FundoTransferencia.FundoTransferenciaRepository.VerificaExistenciaFundoTransferencia("45543915000181", "QA teste");

                        if (fundoTransferenciaExiste)
                        {
                            Console.WriteLine("Fundo de Transferencia adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarFundoTransferencia = Repository.FundoTransferencia.FundoTransferenciaRepository.ApagarFundoTransferencia("45543915000181", "QA teste");

                            if (apagarFundoTransferencia)
                            {
                                Console.WriteLine("Fundo de Transferencia apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Fundo de Transferencia");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir fundo de Transferencia");
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
                    Console.Write("Erro ao carregar a página de Fundos De Transferencia no tópico Cadastro ");
                    pagina.Nome = "Fundos de Transferência";
                    pagina.StatusCode = CadastroFundosTransferencia.Status;
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

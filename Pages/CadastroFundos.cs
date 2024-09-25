using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TestePortal.Utils;
using static TestePortal.Model.Usuario;

namespace TestePortal.Pages
{
    public class CadastroFundos
    {
        public static async Task<Model.Pagina> Fundos(IPage Page, NivelEnum nivelLogado)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var CadastroFundos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Fundos.aspx");

                if (CadastroFundos.Status == 200)
                {
                    string seletorTabela = "#tabelaFundos";

                    Console.Write("Fundos - Cadastro: ");
                    pagina.StatusCode = CadastroFundos.Status;
                    pagina.Nome = "Fundos";
                    listErros.Add("0");
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;

                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    if (nivelLogado == NivelEnum.Master)
                    {

                        await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar Fundo +" }).ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#Nome").ClickAsync();
                        await Task.Delay(300);
                        await Page.Locator("#Nome").FillAsync("teste QA");
                        await Task.Delay(300);
                        await Page.GetByPlaceholder("/0000-00").ClickAsync();
                        await Task.Delay(300);
                        await Page.GetByPlaceholder("/0000-00").FillAsync("45543915000181");
                        await Task.Delay(300);
                        await Page.Locator("#tipoProcessamento").SelectOptionAsync(new[] { "zitec" });
                        await Task.Delay(300);
                        await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                        var fundoExiste = Repository.Fundos.FundosRepository.VerificaExistenciaFundo("45543915000181", "teste QA");

                        if (fundoExiste)
                        {
                            Console.WriteLine("Fundo adicionado com sucesso na tabela.");
                            pagina.InserirDados = "✅";
                            var apagarFundo = Repository.Fundos.FundosRepository.ApagarFundo("45543915000181", "teste QA");

                            if (apagarFundo)
                            {
                                Console.WriteLine("Fundo apagado com sucesso");
                                pagina.Excluir = "✅";
                            }
                            else
                            {
                                Console.WriteLine("Não foi possível apagar Fundo");
                                pagina.Excluir = "❌";
                                errosTotais++;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Não foi possível inserir fundo");
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
                    Console.Write("Erro ao carregar a página de Fundos no tópico Cadastro ");
                    pagina.Nome = "Fundos";
                    errosTotais++;
                    pagina.StatusCode = CadastroFundos.Status;
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

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
    public class CadastroConsultorias
    {
        public static async Task<Model.Pagina> Consultorias (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;
            try
            {
                var CadastroConsultorias = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Consultoria.aspx");

                if (CadastroConsultorias.Status == 200)
                {
                    string seletorTabela = "#tabelaConsultoria";
                    string seletorBotao = "";

                    Console.Write("Consultorias - Cadastro: ");
                    pagina.Nome = "Consultorias";
                    pagina.StatusCode = CadastroConsultorias.Status;
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
                    await Page.Locator("#CnpjConsultoria").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#CnpjConsultoria").FillAsync("53300608000106");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#emailConsultora").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#emailConsultora").FillAsync("jessica.tavares@gmail.com");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();

                    var ConsultoriasExiste = Repository.Consultorias.ConsultoriasRepository.VerificaExistenciaConsultorias("53300608000106", "jessica.tavares@gmail.com");

                    if (ConsultoriasExiste)
                    {
                        Console.WriteLine("Consultoria adicionada com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var ApagarConsultorias = Repository.Consultorias.ConsultoriasRepository.ApagarConsultorias("53300608000106", "jessica.tavares@gmail.com");

                        if (ApagarConsultorias)
                        {
                            Console.WriteLine("Consultoria apagado com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Consultoria");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Consultoria");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }


                }
                else
                {
                    Console.Write("Erro ao carregar a página de Consultorias no tópico Cadastro ");
                    pagina.Nome = "Consultorias";
                    pagina.StatusCode = CadastroConsultorias.Status;
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
    }  //blob:https://portal.idsf.com.br/f59cd5b7-a4a9-4797-8167-b5a013818ef3
}

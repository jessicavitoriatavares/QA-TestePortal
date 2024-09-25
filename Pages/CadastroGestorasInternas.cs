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
    public class CadastroGestorasInternas
    {
        public static async Task<Model.Pagina> GestorasInternas (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var CadastroGestorasInternas = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/GestoraInterno.aspx");

                if (CadastroGestorasInternas.Status == 200)
                {
                    string seletorTabela = "#tabelaGestoras";

                    Console.Write("Gestoras Internas - Cadastro: ");
                    pagina.Nome = "Gestoras Internas";
                    pagina.StatusCode = CadastroGestorasInternas.Status;
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
                    await Task.Delay(300);
                    await Page.Locator("#CnpjGestoraInterno").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#CnpjGestoraInterno").FillAsync("53300608000106");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "AvanÃ§ar" }).ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#emailGestora").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#emailGestora").FillAsync("jessica.tavares@gmail.com");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
                    await Task.Delay(500);

                    var gestoraInternaExiste = Repository.GestoraInterna.GestoraInternaRepository.VerificaExistenciaGestoraInterna("53300608000106", "jessica.tavares@gmail.com");

                    if (gestoraInternaExiste)
                    {
                        Console.WriteLine("Gestora Interna adicionada com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarGestoraInterna = Repository.GestoraInterna.GestoraInternaRepository.ApagarGestoraInterna("53300608000106", "jessica.tavares@gmail.com");

                        if (apagarGestoraInterna)
                        {
                            Console.WriteLine("Gestora Interna apagada com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Gestora Interna");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Gestora Interna");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Gestoras Internas no tópico Cadastro ");
                    pagina.Nome = "Gestoras Internas";
                    pagina.StatusCode = CadastroGestorasInternas.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            } catch (Exception ex)
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

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
    public class BancoIdCorrentista

    {

        public static async Task<Model.Pagina> Correntista (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var PaginaBancoIdCorrentista = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Correntistas.aspx");

                if (PaginaBancoIdCorrentista.Status == 200)
                {
                    Console.Write("Banco Id - Correntista: ");
                    Console.WriteLine(PaginaBancoIdCorrentista.Status);
                    pagina.StatusCode = PaginaBancoIdCorrentista.Status;
                    pagina.Nome = "Banco Id - Correntista ";
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    string seletorTabela = "#tabelaCorrentista";
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Novo +" }).ClickAsync();
                    await Page.Locator("#cpfcnpj").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#cpfcnpj").FillAsync("45543915000181");
                    await Task.Delay(300);
                    await Page.Locator("#tipoContaCorrentista").SelectOptionAsync(new[] { "ESCROW" });
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Avançar" }).ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#paginaModalPJ #emailEmpresa").ClickAsync();
                    await Task.Delay(300);
                    await Page.Locator("#paginaModalPJ #emailEmpresa").FillAsync("carrefur@gmail.com");
                    await Task.Delay(300);
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Cadastrar" }).ClickAsync();
                    await Task.Delay(400);

                    var correntistaExiste = Repository.Correntistas.CorrentistaRepository.VerificaExistenciaCorrentista("carrefur@gmail.com", "45543915000181");

                    if (correntistaExiste)
                    {
                        Console.WriteLine("Correntista adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";

                        var apagarCorrentista = Repository.Correntistas.CorrentistaRepository.ApagarCorrentista("carrefur@gmail.com", "45543915000181");
                        if (apagarCorrentista)
                        {
                            Console.WriteLine("Correntista apagado com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar Correntista");
                            pagina.Excluir = "❌";
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir Correntista");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        errosTotais += 2;
                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Correntista no tópico Banco ID ");
                    Console.WriteLine(PaginaBancoIdCorrentista.Status);
                    listErros.Add("Erro ao carregar a página de Correntista no tópico Banco ID ");
                    pagina.Nome = "Banco Id - Correntista";
                    errosTotais++;
                    pagina.StatusCode = PaginaBancoIdCorrentista.Status;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }

            } catch (TimeoutException ex) 
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

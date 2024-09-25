using Microsoft.Playwright;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using TestePortal.Model;
using TestePortal.Utils;

namespace TestePortal.Utils
{
    public static class Listagem
    {
        public static async Task<string> VerificarListagem(IPage Page, string seletorTabela)
        {
            string Listagem = "❌";

            try
            {

                var tabelaExiste = await Page.QuerySelectorAsync(seletorTabela);
                if (tabelaExiste == null)
                {
                    Console.WriteLine("Tabela não encontrada.");
                    Listagem = "❌";
                }

                await Page.WaitForSelectorAsync($"{seletorTabela}", new PageWaitForSelectorOptions { Timeout = 7000 });
                var tabela = await Page.QuerySelectorAsync(seletorTabela);

                if (tabela != null)
                {
                    
                    var celulaVazia = await Page.QuerySelectorAsync($"{seletorTabela} td.dataTables_empty");

                    if (celulaVazia != null)
                    {
                        var mensagemVazia = await celulaVazia.InnerTextAsync();

                        if (mensagemVazia.Contains("No data available in table"))
                        {
                            Console.WriteLine("Tabela encontrada, mas está vazia (mensagem em inglês).");
                            Listagem = "❗";
                        }
                        else if (mensagemVazia.Contains("Nenhum registro encontrado"))
                        {
                            Console.WriteLine("Tabela encontrada, mas está vazia (mensagem em português).");
                            Listagem = "❗";
                        }
                    }
                    else
                    {
                      
                        var textoTabelaVaziaIngles = await Page.Locator("text='Showing 0 to 0 of 0 entries'").CountAsync();
                        var textoTabelaVaziaPortugues = await Page.Locator("text='Mostrando 0 até 0 de 0 registros'").CountAsync();

                        if (textoTabelaVaziaIngles > 0 || textoTabelaVaziaPortugues > 0)
                        {
                            Console.WriteLine("Tabela encontrada e não possui linhas.");
                            Listagem = "❗";
                        }
                        else
                        {
                           
                            var tbodyExiste = await Page.QuerySelectorAsync($"{seletorTabela} tbody");
                            if (tbodyExiste != null)
                            {
                                
                                await Page.WaitForSelectorAsync($"{seletorTabela} tbody tr", new PageWaitForSelectorOptions { Timeout = 180000 });
                                var linhas = await Page.QuerySelectorAllAsync($"{seletorTabela} tbody tr");
                                var tabelaContemLinhas = linhas.Any(linha => linha != null);

                                if (tabelaContemLinhas)
                                {
                                    Console.WriteLine("Tabela encontrada e possui linhas");
                                    Listagem = "✅";
                                }
                            }
                            else
                            {
                                Console.WriteLine("Tabela sem linhas e sem mensagens de registros vazios.");
                                Listagem = "❗";
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Tabela não encontrada.");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Timeout ao verificar a tabela: {ex.Message}");
                Listagem = "❌";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao verificar a tabela: {ex.Message}");
                Listagem = "❌";
            }

            if (string.IsNullOrEmpty(Listagem))
            {
                Console.WriteLine("ERROR!!!!");
            }

            return Listagem;
        }
    }

}



﻿using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Pages
{
    public class OperacoesConciliacaoExtrato
    {
        public static async Task<Model.Pagina> ConciliacaoExtrato (IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var ConciliacaoExtrato = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/operacoes/ConciliacaoExtrato.aspx");

                if (ConciliacaoExtrato.Status == 200)
                {
                    string seletorTabela = "#tabelaConciliacao";

                    Console.Write("Concilicação e Extrato  - Operações : ");
                    pagina.Nome = "Conciliação e Extrato - Operações";
                    pagina.StatusCode = ConciliacaoExtrato.Status;
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }
                    pagina.BaixarExcel = Utils.Excel.BaixarExcel(Page).Result;
                    if (pagina.BaixarExcel == "❌")
                    {
                        errosTotais++;
                    }

                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                }
                else
                {
                    Console.Write("Erro ao carregar a página de Conciliação e Extrato no tópico Operações: ");
                    pagina.Nome = "Conciliação e Extrato - Operações";
                    pagina.StatusCode = ConciliacaoExtrato.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                pagina.TotalErros = errosTotais;
                return pagina;
            }
            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}

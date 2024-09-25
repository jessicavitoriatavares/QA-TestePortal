﻿using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using TestePortal.Model;

namespace TestePortal.Pages
{
    public class AdministrativoGrupos
    {

        public static async Task<Model.Pagina> Grupos(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var PaginaAdministrativoGrupos = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Permissoes/GrupoPermissoes.aspx");

                if (PaginaAdministrativoGrupos.Status == 200)
                {
                    Console.Write("Administrativo - Grupos: ");
                    Console.WriteLine(PaginaAdministrativoGrupos.Status);

                    pagina.StatusCode = PaginaAdministrativoGrupos.Status;
                    pagina.Nome = "Administrativo Grupos";
                    pagina.Listagem = "❓";
                    pagina.BaixarExcel = "❓";
                    pagina.InserirDados = "❓";
                    pagina.Excluir = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                }

                else
                {
                    Console.Write("Erro ao carregar a página de Grupos no tópico Administrativo ");
                    Console.WriteLine(PaginaAdministrativoGrupos.Status);
                    pagina.Nome = "Administrativo Grupos";
                    pagina.StatusCode = PaginaAdministrativoGrupos.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                errosTotais++;
                return pagina;

            }

            pagina.TotalErros = errosTotais;
            return pagina;
        }
    }
}

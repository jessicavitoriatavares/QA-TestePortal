using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TestePortal.Utils;
using static System.Net.WebRequestMethods;

namespace TestePortal.Pages
{
    public class AdministrativoUsuarios
    {

        public static async Task<Model.Pagina> Usuarios(IPage Page)
        {
            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var PaginaAdministrativoUsuarios = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/Usuarios.aspx");
                if (PaginaAdministrativoUsuarios.Status == 200)
                {
                    // await Listagem.VerificarListagem(Page, seletorTabela); outra forma de chamar o método

                    Console.Write("Administrativo - Usuarios: ");
                    pagina.Nome = "Administrativo Usuarios";
                    pagina.StatusCode = PaginaAdministrativoUsuarios.Status;
                    pagina.BaixarExcel = "❓";
                    pagina.Reprovar = "❓";
                    pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;

                    if (pagina.Acentos == "❌")
                    {
                        errosTotais++;
                    }

                    string seletorTabela = "#tabelaUsuarios";
                    pagina.Listagem = Utils.Listagem.VerificarListagem(Page, seletorTabela).Result;
                    if (pagina.Listagem == "❌")
                    {
                        errosTotais++;
                    }
                    //pagina.ListaErros = listErros;

                    await Page.GetByRole(AriaRole.Button, new() { Name = "Adicionar Usuário +" }).ClickAsync();
                    await Page.Locator("#Nome").ClickAsync();
                    await Page.Locator("#Nome").FillAsync("Jessica Vitoria Tavares");
                    await Page.GetByPlaceholder("Email do SLACK").ClickAsync();
                    await Page.GetByPlaceholder("Email do SLACK").FillAsync("jessica.tav@gmail.com");
                    await Page.Locator("#Nivel").SelectOptionAsync(new[] { "INTERNO" });
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Salvar" }).ClickAsync();

                    var usuarioExiste = Repository.Usuarios.UsuarioRepository.VerificaExistenciaUsuario("Jessica Vitoria Tavares", "jessica.tav@gmail.com");
                   
                    if (usuarioExiste)
                    {
                        Console.WriteLine("Usuário adicionado com sucesso na tabela.");
                        pagina.InserirDados = "✅";
                        var apagarUsuario = Repository.Usuarios.UsuarioRepository.ApagarUsuario("Jessica Vitoria Tavares", "jessica.tav@gmail.com");

                        if (apagarUsuario)
                        {
                            Console.WriteLine("Usuário apagado com sucesso");
                            pagina.Excluir = "✅";
                        }
                        else
                        {
                            Console.WriteLine("Não foi possível apagar usuário");
                            pagina.Excluir = "❌";
                            listErros.Add("Erro ao apagar o usuário.");
                            errosTotais++;
                        }

                    }
                    else
                    {
                        Console.WriteLine("Não foi possível inserir usuário");
                        pagina.InserirDados = "❌";
                        pagina.Excluir = "❌";
                        listErros.Add("Erro ao inserir o usuário.");
                        errosTotais += 2;
                    }

                }
                else
                {
                    Console.Write("Erro ao carregar a página de Usuários no tópico Administrativo ");
                    Console.WriteLine(PaginaAdministrativoUsuarios.Status);
                    listErros.Add($"Erro {PaginaAdministrativoUsuarios.Status} ao carregar a página de Usuários no tópico Administrativo ");
                    pagina.Nome = "Administrativo Usuarios";
                    pagina.StatusCode = PaginaAdministrativoUsuarios.Status;
                    errosTotais++;
                    await Page.GotoAsync("https://portal.idsf.com.br/Home.aspx");
                    pagina.TotalErros = errosTotais;
                    return pagina;
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



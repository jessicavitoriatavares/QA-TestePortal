using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestePortal.Model;
using TestePortal.Utils;

namespace TestePortal.Pages
{


    public class LoginGeral
    {
        public async static Task<Model.Pagina> Login(IPage Page, Usuario usuario)
        {

            var pagina = new Model.Pagina();
            var listErros = new List<string>();
            int errosTotais = 0;

            try
            {
                var PaginaLogin = await Page.GotoAsync(ConfigurationManager.AppSettings["LINK.PORTAL"].ToString() + "/login.aspx");
                await Page.GetByPlaceholder("E-mail").FillAsync(usuario.Email);
                await Page.GetByPlaceholder("Senha").FillAsync(usuario.Senha);
                await Page.GetByRole(AriaRole.Button, new() { Name = "Entrar" }).ClickAsync();
                await Page.WaitForTimeoutAsync(3000);

                if (PaginaLogin.Status == 200)
                {

                    var login = await Page.QuerySelectorAsync("#Home");
                    if (login != null && await login.IsVisibleAsync())
                    {
                        Console.Write("Login: ");
                        Console.WriteLine(PaginaLogin.Status);
                        pagina.Nome = "Login";
                        pagina.StatusCode = PaginaLogin.Status;
                        pagina.Listagem = "❓";
                        pagina.BaixarExcel = "❓";
                        pagina.InserirDados = "❓";
                        pagina.Excluir = "❓";
                        pagina.Reprovar = "❓";
                        pagina.Acentos = Utils.Acentos.ValidarAcentos(Page).Result;
                    }
                    else
                    {
                        var errorLogin = await Page.GetByText("Senha incorreta").ElementHandleAsync();
                        if (errorLogin != null && await errorLogin.IsVisibleAsync())
                        {
                            errosTotais++;
                            EmailPadrao emailPadrao = new EmailPadrao(
                                "jt@zitec.ai",
                                "Erro de Login no Portal IDSF",
                                "O login não teve sucesso. Não é possível fazer as verificações das páginas do portal.",
                                null
                            );

                            EnviarEmail.SendMailWithAttachment(emailPadrao);
                            Console.WriteLine("Senha incorreta detectada e e-mail enviado.");
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                errosTotais++;
                Console.WriteLine($"Ocorreu um erro: {ex.Message}");
            }
            pagina.Perfil = usuario.Nivel.ToString();
            pagina.TotalErros = errosTotais;
            return pagina;
        } 
            
            //"co@zitec.ai,mm@zitec.ai,jt@zitec.ai,mp@zitec.ai,ti@zitec.zi", id2021 -  senha

    }
}

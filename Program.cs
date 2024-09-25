using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestePortal;
using TestePortal.Model;
using TestePortal.Utils;
using TestePortal.Pages;
using System.IO;
using System.Windows.Controls;

namespace TestePortalIDSF
{
    class Program
    {
        public static IPage Page { get; set; }
        public static List<Usuario> Usuarios { get; set; }

        public static async Task Main()
        {

            var playwright = await Playwright.CreateAsync();

            IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions 
            { 
                Channel = "chrome",
                Headless = false, 
                SlowMo = 50, 
                Timeout = 0, 
                Args = new List<string>() { "--window-size=1350,900" }
            });

            Page = await browser.NewPageAsync();
            var listaPagina = new List<Pagina>();

            await browser.NewContextAsync(new BrowserNewContextOptions
            {
                AcceptDownloads = true,
                IgnoreHTTPSErrors = true,
            });

          
            Usuarios = Util.GetUsuariosForTest();


            #region VerificaçãoDeStatusDasPáginas

            foreach (var usuario in Usuarios)
            {
            listaPagina.Add(await TestePortal.Pages.LoginGeral.Login(Page, usuario));

                if (usuario.Nivel == Usuario.NivelEnum.Master)
                {
                    listaPagina.Add(await AdministrativoGrupos.Grupos(Page));
                    listaPagina.Add(await AdministrativoUsuarios.Usuarios(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await BancoIdCorrentista.Correntista(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await BancoIdContasEscrow.ContasEscrow(Page, usuario.Nivel));
                    await Task.Delay(500);
                    listaPagina.Add(await BancoIdReembolso.Reembolso(Page, usuario.Nivel));
                    listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                    listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                    listaPagina.Add(await BancoIdZeragem.Zeragem(Page));
                    listaPagina.Add(await CadastroCarteira.Carteira(Page));
                    listaPagina.Add(await CadastroConsultoras.Consultoras(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroConsultorias.Consultorias(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroCotistas.Cotista(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroInvestidores.Investidores(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroFundos.Fundos(Page, usuario.Nivel));
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroFundosTransferencia.FundosTransf(Page, usuario.Nivel));
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroGestoras.Gestoras(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroGestorasInternas.GestorasInternas(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroPrestServico.PrestServico(Page));
                    listaPagina.Add(await CadastroOfertas.Ofertas(Page));
                    listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                    listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                    listaPagina.Add(await BoletagemAmortizacao.Amortizacao(Page));
                    listaPagina.Add(await CedentesCedentes.Cedentes(Page)); //ajustar o arquivo enviado
                    listaPagina.Add(await CedentesKitCedente.KitCedentes(Page));
                    listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                    await Task.Delay(500);
                    listaPagina.Add(await NotaComercial.NotasComerciais(Page));
                    listaPagina.Add(await NotasInternas.NotassInternas(Page));
                    listaPagina.Add(await OperacoesArquivosBaixa.ArquivosBaixa(Page));
                    listaPagina.Add(await OperacoesAtivos.Ativos(Page, usuario.Nivel));
                    listaPagina.Add(await OperacoesBaixaEmLote.BaixaLote(Page));
                    listaPagina.Add(await OperacoesEnviarLastros.EnviarLastros(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await OperacoesOperacoes.Operacoes(Page));
                    listaPagina.Add(await OperacoesRecebiveis.Recebiveis(Page));
                    listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await OperacoesConciliacaoExtrato.ConciliacaoExtrato(Page));
                    listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                    listaPagina.Add(await RelatorioCedentes.Cedentes(Page));
                    listaPagina.Add(await RelatorioCotistas.Cotistas(Page));
                    listaPagina.Add(await RelatorioFundos.Fundos(Page));
                    listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                    listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
                    listaPagina.Add(await ControleInternoPoliticas.Politicas(Page));
                    listaPagina.Add(await ControleInternoDiario.Diario(Page));
                    await Page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();

                    foreach (var page in listaPagina)
                    {
                        if (page.Perfil == null)
                        page.Perfil = usuario.Nivel.ToString();
                    }

                }
                else if (usuario.Nivel == Usuario.NivelEnum.Gestora)
                {
                    listaPagina.Add(await BancoIdContasEscrow.ContasEscrow(Page, usuario.Nivel));
                    await Task.Delay(500);
                    listaPagina.Add(await BancoIdReembolso.Reembolso(Page, usuario.Nivel));
                    listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                    listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                    listaPagina.Add(await BancoIdZeragem.Zeragem(Page));
                    listaPagina.Add(await CadastroCarteira.Carteira(Page));
                    listaPagina.Add(await CadastroFundos.Fundos(Page, usuario.Nivel)); // não cadastra
                    listaPagina.Add(await CadastroFundosTransferencia.FundosTransf(Page, usuario.Nivel)); // não cadastra
                    await Task.Delay(500);
                    listaPagina.Add(await CadastroPrestServico.PrestServico(Page));
                    listaPagina.Add(await CadastroOfertas.Ofertas(Page));
                    listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                    listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                    listaPagina.Add(await CedentesCedentes.Cedentes(Page)); //ajustar o arquivo enviado
                    listaPagina.Add(await CedentesKitCedente.KitCedentes(Page));
                    listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                    await Task.Delay(500);
                    listaPagina.Add(await NotaComercial.NotasComerciais(Page));
                    listaPagina.Add(await OperacoesArquivosBaixa.ArquivosBaixa(Page));
                    listaPagina.Add(await OperacoesAtivos.Ativos(Page, usuario.Nivel));
                    listaPagina.Add(await OperacoesEnviarLastros.EnviarLastros(Page));
                    listaPagina.Add(await OperacoesOperacoes.Operacoes(Page));
                    listaPagina.Add(await OperacoesRecebiveis.Recebiveis(Page));
                    listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                    listaPagina.Add(await OperacoesConciliacaoExtrato.ConciliacaoExtrato(Page));
                    listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                    listaPagina.Add(await RelatorioCedentes.Cedentes(Page));
                    listaPagina.Add(await RelatorioCotistas.Cotistas(Page));
                    await Task.Delay(500);
                    listaPagina.Add(await RelatorioFundos.Fundos(Page));
                    listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                    listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
                    await Page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();

                    foreach (var page in listaPagina)
                    {
                        if (page.Perfil == null)
                            page.Perfil = usuario.Nivel.ToString();
                    }

                }
                else if (usuario.Nivel == Usuario.NivelEnum.Consultoria)
                {
                    listaPagina.Add(await BancoIdContasEscrow.ContasEscrow(Page, usuario.Nivel)); //não cadastra
                    listaPagina.Add(await BancoIdReembolso.Reembolso(Page, usuario.Nivel));//não cadastra
                    listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                    listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                    listaPagina.Add(await BancoIdZeragem.Zeragem(Page));
                    listaPagina.Add(await CadastroFundos.Fundos(Page, usuario.Nivel));//não cadastra
                    listaPagina.Add(await CadastroPrestServico.PrestServico(Page));
                    listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                    listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                    listaPagina.Add(await CedentesCedentes.Cedentes(Page)); //ajustar o arquivo enviado
                    listaPagina.Add(await CedentesKitCedente.KitCedentes(Page));
                    listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                    listaPagina.Add(await NotaComercial.NotasComerciais(Page));
                    listaPagina.Add(await OperacoesArquivosBaixa.ArquivosBaixa(Page));
                    listaPagina.Add(await OperacoesAtivos.Ativos(Page, usuario.Nivel));
                    listaPagina.Add(await OperacoesEnviarLastros.EnviarLastros(Page));
                    listaPagina.Add(await OperacoesOperacoes.Operacoes(Page));
                    listaPagina.Add(await OperacoesRecebiveis.Recebiveis(Page));
                    listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                    listaPagina.Add(await OperacoesConciliacaoExtrato.ConciliacaoExtrato(Page));
                    listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                    listaPagina.Add(await RelatorioCotistas.Cotistas(Page));
                    listaPagina.Add(await RelatorioFundos.Fundos(Page));
                    listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                    listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
                    await Page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();

                    foreach (var page in listaPagina)
                    {
                        if (page.Perfil == null)
                            page.Perfil = usuario.Nivel.ToString();

                    }

                }
                else if (usuario.Nivel == Usuario.NivelEnum.Denver)
                {
                    listaPagina.Add(await AdministrativoGrupos.Grupos(Page));
                    listaPagina.Add(await BancoIdExtratos.Extratos(Page));
                    listaPagina.Add(await BancoIdSaldos.Saldos(Page));
                    listaPagina.Add(await CadastroFundos.Fundos(Page, usuario.Nivel));
                    await Task.Delay(600);
                    listaPagina.Add(await BoletagemAporte.Aporte(Page, usuario.Nivel));
                    await Task.Delay(600);
                    listaPagina.Add(await BoletagemResgate.Resgate(Page, usuario.Nivel));
                    listaPagina.Add(await NotasPagamentos.Pagamentos(Page, usuario.Nivel));
                    await Task.Delay(500);
                    // listaPagina.Add(await NotaComercial.NotasComerciais(Page));// não aparece na página
                    listaPagina.Add(await OperacoesAtivos.Ativos(Page, usuario.Nivel)); ;
                    listaPagina.Add(await OperacoesConciliacao.Conciliacao(Page));
                    listaPagina.Add(await OperacoesConciliacaoExtrato.ConciliacaoExtrato(Page));
                    listaPagina.Add(await RelatorioCadastro.Cadastro(Page));
                    listaPagina.Add(await RelatorioCedentes.Cedentes(Page));
                    listaPagina.Add(await RelatorioCotistas.Cotistas(Page));
                    listaPagina.Add(await RelatorioFundos.Fundos(Page));
                    listaPagina.Add(await MeusRelatorios.Relatorios(Page));
                    listaPagina.Add(await RelatoriosOperacoes.Operacoes(Page));
                    listaPagina.Add(await ControleInternoPoliticas.Politicas(Page));
                    listaPagina.Add(await ControleInternoDiario.Diario(Page));
                    await Page.GetByRole(AriaRole.Link, new() { Name = " Sair" }).ClickAsync();
                    await Page.GetByRole(AriaRole.Button, new() { Name = "Sim" }).ClickAsync();

                    foreach (var page in listaPagina)
                    {
                        if (page.Perfil == null)
                            page.Perfil = usuario.Nivel.ToString();
                    }

                }

            }

            #endregion


            #region EnviarEmail - Txt

            using (StreamWriter writer = new StreamWriter("C:\\Temp\\Paginas.txt"))
            {
                foreach (var item in listaPagina)
                {
                    writer.WriteLine(item.ToFormattedString());
                    writer.WriteLine(new string('-', 40));
                }
            }

            #endregion

            #region EnviarEmail

            Console.WriteLine("Status Code salvos em Paginas.txt");

            try
            {
                EmailPadrao emailPadrao = new EmailPadrao(
                    "co@zitec.ai,mm@zitec.ai,jt@zitec.ai,mp@zitec.ai,ti@zitec.zi,jk@zitec.ai,lp@zitec.ai", 
                    "Relatório das páginas do portal.",
                    EnviarEmail.GerarHtml(listaPagina),
                    "C:\\Temp\\Paginas.txt"
                  );

                EnviarEmail.SendMailWithAttachment(emailPadrao);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Email enviado");

            await Page.PauseAsync();

            #endregion

           // "co@zitec.ai,mm@zitec.ai,jt@zitec.ai,mp@zitec.ai,ti@zitec.zi,jk@zitec.ai"
        }


    }
}


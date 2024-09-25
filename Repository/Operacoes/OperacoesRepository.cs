using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using TestePortal.Repository.Operacoes;
using System.Windows.Controls;


namespace TestePortal.Repository.Operacoes
{
    public class OperacoesRepository
    {

        public static bool VerificaExistenciaOperacao(string arquivoEntrada)
        {
            var existe = false;

            try
            {
                var con = ConfigurationManager.ConnectionStrings["FROMTISPROC.PRODUCAO.ConnectionString"].ToString();////

                using (SqlConnection myConnection = new SqlConnection(con))
                {
                    myConnection.Open();

                    string query = "SELECT * FROM TB_ARQUIVO WHERE NM_ARQUIVO_ENTRADA = @arquivoEntrada";
                    using (SqlCommand oCmd = new SqlCommand(query, myConnection))
                    {
                        oCmd.Parameters.AddWithValue("@arquivoEntrada", SqlDbType.NVarChar).Value = arquivoEntrada;

                        using (SqlDataReader oReader = oCmd.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                existe = true;

                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Utils.Slack.MandarMsgErroGrupoDev(e.Message, "OperacoesRepository.VerificaExistenciaOperacoes()", "Automações Jessica", e.StackTrace);
            }

            return existe;
        }

        public async Task<bool> ExcluirOperacaoAsync(IPage Page, string nomeArquivo)
        {
            //await Page.EvaluateAsync("document.body.style.zoom = '0.5'");
            try
            {

                await Task.Delay(2000);
                await Page.ReloadAsync(new() { Timeout = 60000 });
                await Page.EvaluateAsync("document.body.style.zoom = '0.5'");
                await Task.Delay(1000);
                await Page.GetByLabel("Pesquisar").ClickAsync(); //botão em inglês em homolog 
                await Task.Delay(2000);
                await Page.GetByLabel("Pesquisar").FillAsync(nomeArquivo);
                var buttons = await Page.Locator("button[title='Excluir Arquivo']").CountAsync();
                if (buttons > 0)
                {
                    await Task.Delay(2000);
                    await Page.Locator("button[title='Excluir Arquivo']").First.ClickAsync();
                }
                await Page.Locator("#motivoExcluirArquivo").FillAsync("teste");
                await Page.GetByRole(AriaRole.Button, new() { Name = "Confirmar" }).ClickAsync();

                // Verifica se a mensagem de sucesso aparece
                var successMessageLocator = Page.Locator("text='Arquivo excluído com sucesso!'");
                bool isMessageVisible = await successMessageLocator.IsVisibleAsync();

                return isMessageVisible;

            }
            catch (TimeoutException ex) {

                Console.WriteLine("Timeout de 2000ms excedido, continuando a execução...");
                Console.WriteLine($"Exceção: {ex.Message}");
                return false;

            }
            
        }


        //caminho para apagar: 
        //await Page.GetByRole(AriaRole.Link, new () { Name = " Operações " }).ClickAsync();
        //await Page.GetByRole(AriaRole.Link, new () { Name = " Operações" }).ClickAsync();
        //await Page.GetByLabel("Pesquisar").ClickAsync();
        //await Page.GetByLabel("Pesquisar").FillAsync("FUNDO QA");
        //await Page.GetByRole(AriaRole.Gridcell, new() { Name = "+ Aprovar Operação" }).ClickAsync();
        //await Page.Locator("button[onclick*=\"ModalExcluirArquivo('120247','78715','CNABQA8.txt')\"]").ClickAsync();
        //await Page.Locator("#motivoExcluirArquivo").ClickAsync();
        //await Page.Locator("#motivoExcluirArquivo").FillAsync("teste");
        //await Page.GetByRole(AriaRole.Button, new () { Name = "Confirmar" }).ClickAsync();


    }
}

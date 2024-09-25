using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestePortal.Model;
using System.Security.Cryptography;
using TestePortal.Pages;
using TestePortal.Utils;
using Microsoft.Playwright;
using System.IO;
using System.Windows.Controls;


namespace TestePortal.Utils
{
    public static class Excel
    {
        public static async Task<string> BaixarExcel(IPage Page)
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string fileName = "PortalIDSF.xlsx";
            string filePath = Path.Combine(downloadPath, fileName);
            string baixarExcel = "";
            var listErros = new List<string>();

            try
            {
                var download = await Page.RunAndWaitForDownloadAsync(async () =>
                {
                    await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync(new LocatorClickOptions
                    {
                        Timeout = 1500
                    });
                    //await Page.Locator(".buttons-excel:has-text('Excel')").ClickAsync();
                });
                if (File.Exists(filePath))
                    File.Delete(filePath);

                await download.SaveAsAsync(filePath);


                if (File.Exists(filePath))
                {
                    Console.WriteLine("Arquivo foi baixado");
                    baixarExcel = "✅";

                    File.Delete(filePath);
                    Console.WriteLine("Arquivo excluído");
                }
                else
                {
                    baixarExcel = "❌";
                    Console.WriteLine("Erro ao baixar arquivo excel");
                    
                }
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"TimeoutException: O download não foi concluído no tempo esperado. Detalhes: {ex.Message}");
                baixarExcel = "❌";
            }
            catch (Exception ex)
            {
       
                Console.WriteLine($"Exceção ao baixar o arquivo: {ex.Message}");
                baixarExcel = "❌";
            }

           
            return baixarExcel;
        }
    }
}


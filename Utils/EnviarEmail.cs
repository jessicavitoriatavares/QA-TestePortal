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
using System.Xml.Schema;

namespace TestePortal.Utils
{
    public static class EnviarEmail
    {

        public static void SendMailWithAttachment(string to, string subject, string body, string fileAttached)
        {
            string from = "no-reply@idgr.com.br";
            string fromName = "No-reply ZITEC";
            string smtp_username = "No-reply IDGR";
            string smtp_password = "AKIAUPAU2HHA2TXXSNHI";
            string host = "smtp.office365.com";//"email-smtp.us-east-1.amazonaws.com";

            InternaAmazonEmailSender(to, subject, body, from, fromName, smtp_username, smtp_password, host, fileAttached);

        }


        public static void SendMailWithAttachment(EmailPadrao emails)
        {
            string from = "no-reply@idsf.com.br";
            string fromName = "No-reply ZITEC";
            string smtp_username = "AKIAUPAU2HHA2TXXSNHI";
            string smtp_password = "BI14sHOsn0aYjv1frsN21UgKgGhfTe+CD8Sk7/vJEdOk";
            string host = "email-smtp.us-east-1.amazonaws.com";//"email-smtp.us-east-1.amazonaws.com";


            InternaAmazonEmailSender(emails.Email, emails.Subject, emails.Body, from, fromName, smtp_username, smtp_password, host, emails.Attached);

        }

        private static void InternaAmazonEmailSender(string to, string subject, string body, string from, string fromName, string smtp_username, string smtp_password, string host, string fileAttached)
        {
            string FROM = from;
            string FROMNAME = fromName;
            string TO = to;
            string SMTP_USERNAME = smtp_username;
            string SMTP_PASSWORD = smtp_password;
            string HOST = host;
            int PORT = 587;
            string SUBJECT = subject;
            string BODY = body;


            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            message.To.Add(to);
            message.Subject = SUBJECT;
            message.Body = BODY;


            if (!string.IsNullOrEmpty(fileAttached))
                message.Attachments.Add(new Attachment(fileAttached));

            using (var client = new SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                // Enable SSL encryption
                client.EnableSsl = true;
                // Try to send the message. Show status in console.
                try
                {
                    // client.TargetName = "smtp.office365.com";
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }

        public static string GerarHtml(List<Pagina> listaPagina)
        {

            Console.WriteLine("A lista contém " + listaPagina.Count + " itens.");
            int totalPaginas = listaPagina.Count;

            var listaMaster = listaPagina.Where(p => p.Perfil == "Master").ToList();
            var listaGestora = listaPagina.Where(p => p.Perfil == "Gestora").ToList();
            var listaConsultoria = listaPagina.Where(p => p.Perfil == "Consultoria").ToList();
            var listaDenver = listaPagina.Where(p => p.Perfil == "Denver").ToList();

            var listaErros = listaPagina.Where(p => p.TotalErros > 0 || p.Listagem == "❌" || p.Acentos == "❌" || p.InserirDados == "❌" || p.Excluir == "❌").ToList();

            string Html = "<html>" +
           "<head>" +
           "<style>" +
           "table {" +
           "   border-collapse: collapse;" +
           "   width: 100%;" +
           "   margin-top: 10px;" +
           "   margin-bottom: 10px;" +
           "}" +
           "th, td {" +
           "   border: 1px solid #dddddd;" +
           "   text-align: left;" +
           "   padding: 8px;" +
           "   background-color: #f2f2f2;" +
           "}" +
           "hr.solid {" +
           "     border-top: 2px solid #bbb;" +
           "}" +
           "</style>" +
           "</head>" +
           "<body>" +
           "<h4>Olá, prezados. Segue o relatório com os status code das páginas e com as verificações solicitadas:</h4>" +
           "<h4>Total de Páginas Verificadas: " + totalPaginas + "</h4>";

            //primeira tabela com o relatório das páginas
            Html += "<h2>Relatório com o usuário: Interno</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome</th><th>Status Code</th><th>Acentos</th><th>Listagem</th><th>BaixarExcel</th><th>InserirDados</th><th>Excluir</th><th>Erros</th></tr>";

            foreach (var pagina in listaMaster)
            {
                Html += "<tr>";
                Html += "<td> " + pagina.Nome + "</td>\n";
                Html += "<td> " + pagina.StatusCode + "</td>\n";
                Html += "<td> " + pagina.Acentos + "</td>\n";
                Html += "<td> " + pagina.Listagem + "</td>\n";
                Html += "<td> " + pagina.BaixarExcel + "</td>\n";
                Html += "<td> " + pagina.InserirDados + "</td>\n";
                Html += "<td> " + pagina.Excluir + "</td>\n";
                Html += "<td> " + pagina.TotalErros + "</td>\n";
                Html += "</tr>";
            }
            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            //segunda tabela com o relatório das páginas
            Html += "<h2>Relatório com o usuário: Gestora</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome</th><th>Status Code</th><th>Acentos</th><th>Listagem</th><th>BaixarExcel</th><th>InserirDados</th><th>Excluir</th><th>Erros</th></tr>";

            foreach (var pagina in listaGestora)
            {
                Html += "<tr>";
                Html += "<td> " + pagina.Nome + "</td>\n";
                Html += "<td> " + pagina.StatusCode + "</td>\n";
                Html += "<td> " + pagina.Acentos + "</td>\n";
                Html += "<td> " + pagina.Listagem + "</td>\n";
                Html += "<td> " + pagina.BaixarExcel + "</td>\n";
                Html += "<td> " + pagina.InserirDados + "</td>\n";
                Html += "<td> " + pagina.Excluir + "</td>\n";
                Html += "<td> " + pagina.TotalErros + "</td>\n";
                Html += "</tr>";
            }
            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            //terceira tabela
            Html += "<h2>Relatório com o usuário: Consultoria</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome</th><th>Status Code</th><th>Acentos</th><th>Listagem</th><th>BaixarExcel</th><th>InserirDados</th><th>Excluir</th><th>Erros</th></tr>";

            foreach (var pagina in listaConsultoria)
            {
                Html += "<tr>";
                Html += "<td> " + pagina.Nome + "</td>\n";
                Html += "<td> " + pagina.StatusCode + "</td>\n";
                Html += "<td> " + pagina.Acentos + "</td>\n";
                Html += "<td> " + pagina.Listagem + "</td>\n";
                Html += "<td> " + pagina.BaixarExcel + "</td>\n";
                Html += "<td> " + pagina.InserirDados + "</td>\n";
                Html += "<td> " + pagina.Excluir + "</td>\n";
                Html += "<td> " + pagina.TotalErros + "</td>\n";
                Html += "</tr>";
            }
            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            //quarta tabela

            Html += "<h2>Relatório com o usuário: Denver</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome</th><th>Status Code</th><th>Acentos</th><th>Listagem</th><th>BaixarExcel</th><th>InserirDados</th><th>Excluir</th><th>Erros</th></tr>";

            foreach (var pagina in listaDenver)
            {
                Html += "<tr>";
                Html += "<td> " + pagina.Nome + "</td>\n";
                Html += "<td> " + pagina.StatusCode + "</td>\n";
                Html += "<td> " + pagina.Acentos + "</td>\n";
                Html += "<td> " + pagina.Listagem + "</td>\n";
                Html += "<td> " + pagina.BaixarExcel + "</td>\n";
                Html += "<td> " + pagina.InserirDados + "</td>\n";
                Html += "<td> " + pagina.Excluir + "</td>\n";
                Html += "<td> " + pagina.TotalErros+ "</td>\n";
                Html += "</tr>";
            }
            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            //tabela com os erros 

            Html += "<h2>Resumo de Páginas com Erros (Perfil: Master)</h2>";
            Html += "<table>";
            Html += "<tr><th>Nome da Página</th><th>Erro</th></tr>";

            foreach (var pagina in listaErros.Where(p => p.Perfil == "Master"))
            {
                var erros = new List<string>();

                // Adiciona erros específicos
                if (pagina.Listagem == "❌") erros.Add("Listagem");
                if (pagina.Acentos == "❌") erros.Add("Acentos");
                if (pagina.BaixarExcel == "❌") erros.Add("BaixarExcel");
                if (pagina.InserirDados == "❌") erros.Add("InserirDados");
                if (pagina.Excluir == "❌") erros.Add("Excluir");
                //if (pagina.TotalErros > 0) erros.Add("Total de Erros: " + pagina.TotalErros);

                // Adiciona à tabela
                Html += "<tr>";
                Html += "<td> " + pagina.Nome + "</td>\n";
                Html += "<td>" + string.Join(", ", erros) + "</td>\n"; // Lista de erros
                Html += "</tr>";
            }

            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            //Legenda

            Html += "<h2>Legenda</h2>";
            Html += "<table>";
            Html += "<tr><td>✅</td><td>OK</td></tr>";
            Html += "<tr><td>❓</td><td>Não se aplica</td></tr>";
            Html += "<tr><td>❌</td><td>Erro</td></tr>";
            Html += "<tr><td>❗</td><td>Verificar/Atenção</td></tr>";
            Html += "</table>";
            Html += "<br>";
            Html += "<hr class=\"solid\">";

            Html += "</body></html>";
            return Html;



        }


    }

}

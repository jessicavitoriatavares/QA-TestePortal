using SlackBotMessages.Models;
using SlackBotMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Utils
{
    public class Slack
    {
        public static void MandarMsgErroGrupoDev(string mensagemErro, string Arquivo, string Projeto, string StackTrace)
        {
            var client = new SbmClient("https://hooks.slack.com/services/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");
            var msg = new Message();
            msg.Text = String.Format("O Erro no projeto {0} referente ao arquivo {1}.", Projeto, Arquivo);
            SlackBotMessages.Models.Attachment attachment_S = new SlackBotMessages.Models.Attachment();
            attachment_S.AddField(Emoji.X + "  Observação:", "", false);
            attachment_S.AddField("MensagemError: ", mensagemErro, false);
            attachment_S.AddField("StackTrace: ", StackTrace, false);
            attachment_S.SetColor(SlackBotMessages.Enums.Color.Red);
            attachment_S.AuthorName = ":logo-id: Portal IDSF :logo-id:";
            attachment_S.AuthorLink = "https://portal.idsf.com.br/";
            msg.AddAttachment(attachment_S);
            client.Send(msg);
        }
    }
}

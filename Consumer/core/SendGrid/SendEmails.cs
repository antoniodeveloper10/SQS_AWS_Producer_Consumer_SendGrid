using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace work.core.SendGrid
{
    public static class SendEmails
    {

        public static bool Send(ISendGridClient sendGridClient, email dadosMSG)
        {
            var message = new SendGridMessage
            {
                Subject = dadosMSG.assunto,
                PlainTextContent = dadosMSG.conteudo,
                From = new EmailAddress(dadosMSG.remetente),
            };

            message.AddTo(dadosMSG.destinatario);
            return sendGridClient.SendEmailAsync(message).Result.IsSuccessStatusCode;


        }


    }
}

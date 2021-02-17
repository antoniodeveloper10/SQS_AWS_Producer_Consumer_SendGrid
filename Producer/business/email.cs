using System;
using System.Collections.Generic;
using System.Text;

namespace Producer.business
{
    class email
    {
        public string assunto { get; set; }
        public string remetente { get; set; }
        public string destinatario { get; set; }
        public string conteudo { get; set; }

        public email(string assunto, string remetente, string destinatario, string conteudo)
        {
            this.assunto = assunto;
            this.remetente = remetente;
            this.destinatario = destinatario;
            this.conteudo = conteudo;

        }
    }
}

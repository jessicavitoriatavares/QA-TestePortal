using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestePortal.Model
{
    public class EmailPadrao
    {
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Attached { get; set; }

        public EmailPadrao(string email, string subject, string body, string attached)
        {
            Email = email;
            Subject = subject;
            Body = body;
            Attached = attached;
        }
    }
}

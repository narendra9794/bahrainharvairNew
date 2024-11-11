using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Model.EmailModel
{
    public class EmailModel
    {

      
    }
    public class SMTPConfigModel
    {
        [EmailAddress]
        public string SenderAddress { get; set; }
        public string SenderDisplayName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool IsHTMLBody { get; set; }

        [EmailAddress]
        public string? CC { get; set; }

        [EmailAddress]
        public string? TestEmailTo { get; set; }
    }

    public class UserMailOptions
    {
        public List<string> ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public List<KeyValuePair<string, string>> PlaceHolders { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.DBCollections
{
    public class Email : BaseEntity
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
        public string CC { get; set; }

        [EmailAddress]
        public string TestEmailTo { get; set; }
    }
}

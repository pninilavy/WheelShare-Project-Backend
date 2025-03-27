using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IEmailService
    {
        public Task<bool> SendEmailAsync(string toEmail, string subject, string plainTextContent, string htmlContent);

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Service.Contarct
{
   public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}

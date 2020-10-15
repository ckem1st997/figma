﻿using System.Threading.Tasks;

namespace figma.Interface
{
    public interface IMailer
    {
        public Task SendEmailSync(string email, string subject, string body);
    }
}

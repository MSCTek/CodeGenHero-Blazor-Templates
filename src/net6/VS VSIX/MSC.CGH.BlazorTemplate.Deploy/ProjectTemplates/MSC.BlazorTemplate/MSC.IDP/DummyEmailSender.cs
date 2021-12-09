using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
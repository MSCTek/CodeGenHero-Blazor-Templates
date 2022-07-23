using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace ArtistSite.IDP
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
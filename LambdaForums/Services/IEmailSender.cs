using System.Threading.Tasks;

namespace LambdaForums.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

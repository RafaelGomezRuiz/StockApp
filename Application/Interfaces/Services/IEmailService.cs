using StockApp.Core.Application.Dtos.Email;

namespace StockApp.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}

using System.Threading.Tasks;

namespace Procurement.Infrastructure.Services
{
    public interface IEmailService
    {
        Task<bool> SendPurchaseOrderNotificationAsync(string toEmail, string subject, string body);
        Task<bool> SendVendorNotificationAsync(string toEmail, string subject, string body);
    }
}
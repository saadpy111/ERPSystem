using System.Threading.Tasks;

namespace Procurement.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> SendPurchaseOrderNotificationAsync(string toEmail, string subject, string body)
        {
            // In a real implementation, this would send an actual email
            // For now, we'll just simulate the operation
            await Task.Delay(100); // Simulate network delay
            return true; // Simulate successful email send
        }
        
        public async Task<bool> SendVendorNotificationAsync(string toEmail, string subject, string body)
        {
            // In a real implementation, this would send an actual email
            // For now, we'll just simulate the operation
            await Task.Delay(100); // Simulate network delay
            return true; // Simulate successful email send
        }
    }
}
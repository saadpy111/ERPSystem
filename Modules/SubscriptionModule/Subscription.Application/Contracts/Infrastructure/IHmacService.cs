namespace Subscription.Application.Contracts.Infrastructure
{
    public interface IHmacService
    {
        bool VerifyHmac(string payload, string hmacSignature);
    }
}

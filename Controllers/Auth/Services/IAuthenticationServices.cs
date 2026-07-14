namespace ShibAPI.Controllers.Auth.Services
{
    public interface IAuthenticationServices
    {
        AuthenticationResult ConnectMail(string WalletAddress, int UserId, string username, string Email,DateTime Expirydate, bool isNewUser, bool Success);

        AuthenticationResult ConnectWallet(string WalletAddress, int UserId, string username, string Email, DateTime Expirydate, bool isNewUser, bool Success);
    }
}

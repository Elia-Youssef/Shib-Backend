namespace ShibAPI.Controllers.Auth.Services
{
    public record AuthenticationResult(
        string WalletAddress,
        int UserId,
        string UserName,
        string Email,
        DateTime Expirydate,
        bool isNewUser,
        bool Success,
        string token
        );
}

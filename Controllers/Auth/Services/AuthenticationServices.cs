using Newtonsoft.Json.Linq;

namespace ShibAPI.Controllers.Auth.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationServices(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator; 
        }

        public AuthenticationResult ConnectWallet(string WalletAddress, int UserId, string username, string Email, DateTime Expirydate, bool isNewUser, bool Success)
        {
            var token = _jwtTokenGenerator.GenerateJwtTokenConnectWallet(UserId, username,WalletAddress);

            return new AuthenticationResult(
                                            WalletAddress,
                                            UserId,
                                            username,
                                            Email,
                                            Expirydate,
                                            isNewUser,
                                            Success,
                                            token.Result);
        }

        public AuthenticationResult ConnectMail( string WalletAddress, int UserId, string username, string Email,DateTime Expirydate, bool isNewUser, bool Success)
        {
            //Guid UserId = Guid.NewGuid();
            var token = _jwtTokenGenerator.GenerateJwtTokenConnectMail(UserId, username,Email);

           return new AuthenticationResult(
                                           WalletAddress,
                                           UserId,
                                           username,
                                           Email,
                                           Expirydate,
                                           isNewUser,
                                           Success,
                                           token.Result);
        }
    }
}

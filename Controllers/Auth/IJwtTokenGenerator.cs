
    public interface IJwtTokenGenerator
    {
    Task<string> GenerateJwtTokenConnectWallet(int Id, string username, string walletAddress);
    Task<string> GenerateJwtTokenConnectMail(int Id, string username, string email);
}


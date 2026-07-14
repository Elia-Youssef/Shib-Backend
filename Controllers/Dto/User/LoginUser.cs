
public class UserWallet
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? WalletAddress { get; set; }

}

public class UserMachine
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? MachineId { get; set; }

    public string? ClientKey { get; set; }

    public string? TokenID { get; set; }
    
    public bool? isNewUser { get; set; }
    
    public bool? isLogged { get; set; }
    
    public DateTime? EffectiveDate { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
}

public class User
{
    public int Id { get; set; }
    
    public string? UserCode { get; set; }
    
    public string? UserName { get; set; }
    
    public string? Email { get; set; }
    
    public int? NftId { get; set; }
    
    public int? CountryId { get; set; }

    private DateTime _createdOn;

    public DateTime CreatedOn
    {
        get => _createdOn;
        set => _createdOn = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

}

public class UserNFT
{
    public int Id { get; set; }

    public int? UserId { get; set; }
    public int? NFTId { get; set; }
    public bool? isDefault { get; set; }

}

public class Params
{
    public string ClientKey { get; set; }
    public string WalletAddress { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? name { get; set; }

}
public class ParamsUser
{
    public string ClientKey { get; set; }
    public string WalletAddress { get; set; }
    //public string Token { get; set; }
    public string UserCode { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool isNewUser { get; set; }
    public bool Success { get; set; }

}

    public class LoginUser
{
    public List<Params> Params { get; set; }


}
public class infoUser
{

    public List<User> UserInfo { get; set; }
  


}
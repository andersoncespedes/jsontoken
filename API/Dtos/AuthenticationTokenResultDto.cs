namespace API.Dtos;

public class AuthenticationTokenResultDto
{
    public string AccesToken {get; set;}
    public string RefreshToken {get; set;}
    public DateTime Expiry {get;set;}   
}

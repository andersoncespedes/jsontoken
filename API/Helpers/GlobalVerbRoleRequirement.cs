
using Microsoft.AspNetCore.Authorization;

namespace API.Helpers;

public class GlobalVerbRolseRequirement : IAuthorizationRequirement
{
    public bool IsAllowed(string Rolse, string verb){
        if(string.Equals("Administrador",Rolse, StringComparison.OrdinalIgnoreCase)) return true;
        if(string.Equals("Gerente",Rolse, StringComparison.OrdinalIgnoreCase)) return true;
        if(string.Equals("empleado",Rolse, StringComparison.OrdinalIgnoreCase) && string.Equals("GET", verb, StringComparison.OrdinalIgnoreCase)){
            return true;
        }
        if(string.Equals("camper",Rolse, StringComparison.OrdinalIgnoreCase) && string.Equals("GET", verb, StringComparison.OrdinalIgnoreCase)){
            return true;
        }
        return false;
    }
}


using Microsoft.AspNetCore.Authorization;

namespace API.Helpers;

public class GlobalVerbRolseRequirement : IAuthorizationRequirement
{
    public bool IsAllowed(string Rol, string verb){
        if(string.Equals("Administrador",Rol, StringComparison.OrdinalIgnoreCase)) return true;
        if(string.Equals("admin",Rol, StringComparison.OrdinalIgnoreCase)) return true;
        if(string.Equals("empleado",Rol, StringComparison.OrdinalIgnoreCase) && string.Equals("POST", verb, StringComparison.OrdinalIgnoreCase)){
            return true;
        }
        if(string.Equals("camper",Rol, StringComparison.OrdinalIgnoreCase) && string.Equals("POST", verb, StringComparison.OrdinalIgnoreCase)){
            return true;
        }
        return false;
    }
}

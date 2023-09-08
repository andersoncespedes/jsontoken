

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace API.Helpers;

public class GlobalVerbRolseHandler : AuthorizationHandler<GlobalVerbRolseRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GlobalVerbRolseHandler(IHttpContextAccessor httpContextAccessor){
        _httpContextAccessor = httpContextAccessor;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GlobalVerbRolseRequirement requirement)
    {
        var Rolses = context.User.FindAll(e => string.Equals(e.Type, ClaimTypes.Role)).Select(e => e.Value);
        var verb = _httpContextAccessor.HttpContext?.Request.Method;
        if(String.IsNullOrEmpty(verb)){throw new Exception($"request can't be null");}
        foreach(var Rolse in Rolses){

            if(requirement.IsAllowed(Rolse,verb)){
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }
        context.Fail();
        return Task.CompletedTask;
    }

}

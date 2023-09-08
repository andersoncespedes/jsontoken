using Microsoft.EntityFrameworkCore;
using Dominio.Entities;
using Dominio.Interfaces;

using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Dtos;

namespace API.ContRolslers;

public class UserContRolsler : BaseApiContRolsler
{
    private readonly IUserService _userService;
    public UserContRolsler(IUserService userService){
        _userService = userService;
    }
    [HttpPost("register")]
    public async Task<ActionResult<RegisterDto>> Post(RegisterDto model){
        var datos = await _userService.RegisterAsync(model);
        return Ok(datos);
    }
    [HttpPost("token")]
    public async Task<IActionResult> GetTokenAsync(LoginDto login){
        var token = await _userService.GetTokenAsync(login);
        return Ok(token);
    }
    [HttpPost("addRol")]
    public async Task<IActionResult> AddRolseAsync(AddRoleDto model){
        var result = await _userService.AddRolseAsync(model);
        return Ok(result);
    }

}

using Microsoft.AspNetCore.Mvc;
using API.Services;
using API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Dominio.Interfaces;
using System.Text.RegularExpressions;

namespace API.Controllers;

public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    public UserController(IUserService userService, IUnitOfWork unitOfWork){
        _userService = userService;
        _unitOfWork = unitOfWork;
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
    [HttpGet("a")]
    public async Task<IActionResult> Get(){
        var result = await _unitOfWork.User.GetAllAsync();
        Regex regex = new("^([a-z.]){1,8}$", RegexOptions.IgnoreCase);
        var filtro = result.Where(e => {
            return regex.IsMatch(e.Username);
        })
        .Select(e => {
            e.Username = e.Username.Remove(0,1);
            return e;
        })
        .ToList();
        return Ok(filtro);
    }
}

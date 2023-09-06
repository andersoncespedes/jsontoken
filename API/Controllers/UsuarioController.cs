using Microsoft.EntityFrameworkCore;
using Dominio.Entities;
using Dominio.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UsuarioController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    public UsuarioController(IUnitOfWork unitOfWork){
        _unitOfWork = unitOfWork;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<User>>> Get(){
        var datos = await _unitOfWork.Usuario.GetAllAsync();
        return Ok(datos);
    }

}

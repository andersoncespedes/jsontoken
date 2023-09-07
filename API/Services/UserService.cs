using Dominio.Entities;
using System.IdentityModel.Tokens.Jwt;
using API.Dtos;
using API.Helpers;
using Dominio.Entities;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;


namespace API.Services;

public class UserService : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt, IPasswordHasher<User> passwordHasher){
        _unitOfWork = unitOfWork;
        _jwt = jwt.Value;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> RegisterAsync(RegisterDto model)
    {
        var Persona = new User {
            Username = model.Username,
            Email = model.Email,
        };
        Persona.Password = _passwordHasher.HashPassword(Persona,model.Password);
        var UsuarioExistente = _unitOfWork.Usuario
        .Find(e => e.Username.ToLower() == model.Username.ToLower())
        .FirstOrDefault();
        if(UsuarioExistente == null){
            var rolPredeterminado = _unitOfWork.Rol.Find(e => e.Nombre == "Default")
            .First();
            try{
                Persona.Rols.Add(rolPredeterminado);
                _unitOfWork.Usuario.Add(Persona);
                await _unitOfWork.SaveAsync();
                return $"El usuario {model.Username} fue registrado exitosamente";
            }catch(Exception err){
                var message = err.Message;
                return $"error {message}";
            }
        }
        else{
            return $"el usuario {model.Username} ya se encuentra registrado";
        }
    }
    public async Task<DatosUsuarioDto> GetTokenAsync(LoginDto model){
        DatosUsuarioDto datosUsuarioDto = new DatosUsuarioDto();
        var usuario = await _unitOfWork.Usuario
        .GetByUsernameAsync(model.UserName);
        if(usuario == null){
            datosUsuarioDto.EstaAutentificado = false;
            datosUsuarioDto.Mensaje = $"No existe ningun usuario con el username {model.UserName}";
            return datosUsuarioDto;
        }
        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Password, model.Password);
        if(resultado == PasswordVerificationResult.Success){
            datosUsuarioDto.EstaAutentificado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
            datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosUsuarioDto.Username = model.UserName;
            datosUsuarioDto.Roles = usuario.Rols.Select(e => e.Nombre).ToList();
            return datosUsuarioDto;
        }
        datosUsuarioDto.EstaAutentificado = false;
        datosUsuarioDto.Mensaje = $"Credenciales Incorrectas";
        return datosUsuarioDto;
    }
    private JwtSecurityToken CreateJwtToken(User user){
        var roles = user.Rols;
        var roleClaims = new List<Claim> ();
        foreach(var role in roles){
            roleClaims.Add(new Claim("roles", role.Nombre));
        }
        var claims = new []{
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id.ToString()),
        }.Union(roleClaims);
        var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer:_jwt.Issuer,
            audience:_jwt.Audience,
            claims:claims,
            expires:DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials:signingCredentials);
        return jwtSecurityToken;
    }

    public async Task<string> AddRoleAsync(AddRoleDto model)
    {
        var usuario = await _unitOfWork.Usuario.GetByUsernameAsync(model.Username);
        if(usuario == null){
            return $"no existe algun usuario registrado con la cuenta {model.Username}";
        }
        var resultado = _passwordHasher.VerifyHashedPassword(usuario,usuario.Password,model.Password);

    }
}

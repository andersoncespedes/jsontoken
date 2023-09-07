using Dominio.Entities;
using System.IdentityModel.Tokens.Jwt;
using API.Dtos;
using API.Helpers;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Net;


namespace API.Services;

public class UserService : IUserService
{
    private readonly JWT _jwt;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IUnitOfWork unitOfWork, IOptions<JWT> jwt,
        IPasswordHasher<User> passwordHasher)
    {
        _jwt = jwt.Value;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        var User = new User
        {

            Email = registerDto.Email,
            Username = registerDto.Username,

        };

        User.Password = _passwordHasher.HashPassword(User, registerDto.Password);

        var UserExiste = _unitOfWork.User
                                    .Find(u => u.Username.ToLower() == registerDto.Username.ToLower())
                                    .FirstOrDefault();

        if (UserExiste == null)
        {
           // var RolsPredeterminado = _unitOfWork.Rols
                                   // .Find(u => u.Nombre == Authorization.Rol_predeterminado.ToString())
                                    //.First();
            try
            {
                //User.Rolss.Add(RolsPredeterminado);
                _unitOfWork.User.Add(User);
                await _unitOfWork.SaveAsync();

                return $"El User  {registerDto.Username} ha sido registrado exitosamente";
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return $"Error: {message}";
            }
        }
        else
        {
            return $"El User con {registerDto.Username} ya se encuentra registrado.";
        }
    }


    /*public async Task<DatosUserDto> GetTokenAsync(LoginDto model)
    {
        DatosUserDto datosUserDto = new DatosUserDto();
        var User = await _unitOfWork.Users
                    .GetByUsernameAsync(model.Username);

        if (User == null)
        {
            datosUserDto.EstaAutenticado = false;
            datosUserDto.Mensaje = $"No existe ningún User con el username {model.Username}.";
            return datosUserDto;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(User, User.Password, model.Password);

        if (resultado == PasswordVerificationResult.Success)
        {
            datosUserDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(User);
            datosUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosUserDto.Email = User.Email;
            datosUserDto.UserName = User.Username;
            datosUserDto.Rolses = User.Rolses
                                            .Select(u => u.Nombre)
                                            .ToList();
            return datosUserDto;
        }
        datosUserDto.EstaAutenticado = false;
        datosUserDto.Mensaje = $"Credenciales incorrectas para el User {User.Username}.";
        return datosUserDto;
    }*/

    public async Task<string> AddRolseAsync(AddRoleDto model)
    {
        var User = await _unitOfWork.User
                    .GetByUsernameAsync(model.Username);
        if (User == null)
        {
            return $"No existe algún User registrado con la cuenta {model.Username}.";
        }
        var resultado = _passwordHasher.VerifyHashedPassword(User, User.Password, model.Password);
        if (resultado == PasswordVerificationResult.Success)
        {
            var RolsExiste = _unitOfWork.Rols
                                        .Find(u => u.Nombre.ToLower() == model.Rol.ToLower())
                                        .FirstOrDefault();
            if (RolsExiste != null)
            {
                var UserTieneRols = User.Rolss
                                            .Any(u => u.Id == RolsExiste.Id);

                if (UserTieneRols == false)
                {
                    User.Rolss.Add(RolsExiste);
                    _unitOfWork.User.Update(User);
                    await _unitOfWork.SaveAsync();
                }
                return $"Rols {model.Rol} agregado a la cuenta {model.Username} de forma exitosa.";
            }
            return $"Rols {model.Rol} no encontrado.";
        }
        return $"Credenciales incorrectas para el User {User.Username}.";
    }
    public async Task<DatosUserDto> GetTokenAsync(LoginDto model)
    {
        DatosUserDto datosUserDto = new DatosUserDto();
        var User = await _unitOfWork.User
                    .GetByUsernameAsync(model.Username);
        if (User == null)
        {
            datosUserDto.EstaAutenticado = false;
            datosUserDto.Mensaje = $"No existe ningún User con el username {model.Username}.";
            return datosUserDto;
        }

        var result = _passwordHasher.VerifyHashedPassword(User, User.Password, model.Password);
        if (result == PasswordVerificationResult.Success)
        {

            datosUserDto.Mensaje = "Ok";
            datosUserDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(User);
            datosUserDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosUserDto.Username = User.Username;
            datosUserDto.Email = User.Username;
            //datosUserDto.Token = _jwtGenerador.CrearToken(User);
            return datosUserDto;

        }
        datosUserDto.EstaAutenticado = false;
        datosUserDto.Mensaje = $"Credenciales incorrectas para el User {User.Username}.";
        return datosUserDto;

    }
    private JwtSecurityToken CreateJwtToken(User User)
    {
        var Rolses = User.Rolss;
        var RolseClaims = new List<Claim>();
        foreach (var Rolse in Rolses)
        {
            RolseClaims.Add(new Claim("Rolses", Rolse.Nombre));
        }
        var claims = new[]
        {
                                new Claim(JwtRegisteredClaimNames.Sub, User.Username),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Email, User.Email),
                                new Claim("uid", User.Id.ToString())
                        }
        .Union(RolseClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        Console.WriteLine("", symmetricSecurityKey);
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials: signingCredentials);
        return jwtSecurityToken;
    }

    public async Task<LoginDto> UserLogin(LoginDto model)
    {
        var User = await _unitOfWork.User.GetByUsernameAsync(model.Username);
        var resultado = _passwordHasher.VerifyHashedPassword(User, User.Password, model.Password);

        if (resultado == PasswordVerificationResult.Success)
        {
            return model;
        }
        return null;
    }
}
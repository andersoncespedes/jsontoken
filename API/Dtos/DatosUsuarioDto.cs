using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

    public class DatosUsuarioDto
    {
        public string Mensaje {get; set;}
        public bool EstaAutentificado {get; set;}
        public string Username {get; set;}
        public string Email {get; set;}
        public List<string> Roles {get; set;}
        public string Token {get; set;}
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos;

    public class DatosUserDto
    {
        public string Mensaje {get; set;}
        public bool EstaAutenticado {get; set;}
        public string Username {get; set;}
        public string Email {get; set;}
        public List<string> Roles {get; set;}
        public string Token {get; set;}
        public string  AccessToken {get; set;}
        public string  RefreshToken {get; set;}
        public DateTime Expiry {get; set;}
    }

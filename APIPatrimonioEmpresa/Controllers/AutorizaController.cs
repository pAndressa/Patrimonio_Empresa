﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APIPatrimonioEmpresa.Models;
using APIPatrimonioEmpresa.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace APIPatrimonioEmpresa.Controllers
{
    [Route("[controller]")]
    [ApiController]    
    public class AutorizaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UsuarioRepositorio _usuarioRepositorio = new UsuarioRepositorio();

        public AutorizaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }        
        
        [HttpPost]
        public ActionResult RegitrarUsuario([FromBody] Usuario usuario)
        {
            try
            {
                _usuarioRepositorio.IncluirUsuario(usuario);
                return Ok(GeraToken(usuario));                
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Não foi possível realizar o cadastro....");
                return BadRequest(ModelState);
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult LoginUsuario([FromBody]Usuario usuario)
        {
           
                _usuarioRepositorio.LoginUsuario(usuario);
                return Ok(GeraToken(usuario));
          
        }        
        
        public string GeraToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, usuario.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)                
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
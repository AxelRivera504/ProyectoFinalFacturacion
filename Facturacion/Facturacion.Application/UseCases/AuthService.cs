using BCrypt.Net;
using Facturacion.Application.Dtos.Auth;
using Facturacion.Application.Exceptions;
using Facturacion.Application.Interfaces;
using Facturacion.Domain.Entities;
using Facturacion.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Application.UseCases
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PassworHash))
                throw new BusinessException("Credenciales son incorrectas");

            return GenerarToken(usuario);
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            bool existeEmail = await _usuarioRepository.ExisteEmailAsync(registerDto.Email);
            if (existeEmail)
                throw new BusinessException("El email ya esta registrado");

            var usuario = new Usuario
            {
                Nombre = registerDto.Nombre,
                Rol = registerDto.Rol,
                Email = registerDto.Email,  
                PassworHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            await _usuarioRepository.AddAsync(usuario);
        }


        private TokenDto GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration!["Jwt:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expira = DateTime.UtcNow.AddHours(8);

            //Construit el token
            var token = new JwtSecurityToken(
                issuer: _configuration!["Jwt:Issuer"],
                audience: _configuration!["Jwt:Audience"],
                claims: claims,
                expires: expira,
                signingCredentials: creds);

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Nombre = usuario.Nombre,
                Email = usuario.Email,  
                Expira = expira,
                Rol = usuario.Rol
            };
        }
    }
}

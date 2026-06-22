using Facturacion.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Application.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task RegisterAsync(RegisterDto registerDto);
    }
}

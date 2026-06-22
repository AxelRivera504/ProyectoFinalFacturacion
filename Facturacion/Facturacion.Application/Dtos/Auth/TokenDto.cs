using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Application.Dtos.Auth
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public DateTime Expira { get; set; }
    }
}

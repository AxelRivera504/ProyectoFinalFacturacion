using Facturacion.Domain.Entities;
using Facturacion.Domain.Interfaces;
using Facturacion.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly FacturacionContext _facturacionContext;
        public UsuarioRepository(FacturacionContext facturacionContext)
        {
            _facturacionContext = facturacionContext;
        }
        public async Task AddAsync(Usuario usuario)
        {
            await _facturacionContext.AddAsync(usuario);
            await _facturacionContext.SaveChangesAsync();
        }

        public async Task<bool> ExisteEmailAsync(string email)
        => await _facturacionContext.Usuarios.AnyAsync(x => x.Email.ToLower() == email.ToLower());

        public async Task<Usuario?> GetByEmailAsync(string email)
        => await _facturacionContext.Usuarios.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }
}

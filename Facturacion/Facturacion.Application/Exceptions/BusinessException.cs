using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Application.Exceptions
{
    public class BusinessException : Exception
    {
        /// <summary>
        /// Se lanza cuando se viola una regla de negocio.
        /// El middleware lo convierte en 400 Bad Request.
        /// </summary>
        public BusinessException(string mensaje)
        : base(mensaje) {}
    }
}

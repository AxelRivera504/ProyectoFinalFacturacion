using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Se lanza cuando no se encuentra un recurso en la BD.
        /// El middleware lo convierte en 404 Not Found.
        /// </summary>
        public NotFoundException(string entidad, int id) 
        : base($"{entidad} con ID {id} no fue encontrado") { }

        public NotFoundException(string mensaje)
        : base(mensaje) { }

    }
}

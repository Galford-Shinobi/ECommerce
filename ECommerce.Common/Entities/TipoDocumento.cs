using System;
using System.Collections.Generic;

namespace ECommerce.Common.Entities
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Proveedors = new HashSet<Proveedor>();
        }

        public int TipoDocumentoId { get; set; }
        public string Descripcion { get; set; }
        public int? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<Proveedor> Proveedors { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ECommerce.Common.Entities
{
    public partial class Bodega
    {
        public Bodega()
        {
            BodegaProductos = new HashSet<BodegaProducto>();
        }

        public int BodegaId { get; set; }
        public string Descripcion { get; set; }
        public int? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual ICollection<BodegaProducto> BodegaProductos { get; set; }
    }
}

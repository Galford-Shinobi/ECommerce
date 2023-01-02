using System;
using System.Collections.Generic;

namespace ECommerce.Common.Entities
{
    public partial class BodegaProducto
    {
        public int Idproducto { get; set; }
        public int BodegaId { get; set; }
        public decimal Stock { get; set; }
        public decimal Minimo { get; set; }
        public decimal Maximo { get; set; }
        public int DiasReposicion { get; set; }
        public decimal CantidadMinima { get; set; }

        public virtual Bodega Bodega { get; set; }
        public virtual Producto IdproductoNavigation { get; set; }
    }
}

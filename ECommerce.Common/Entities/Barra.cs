using System;
using System.Collections.Generic;

namespace ECommerce.Common.Entities
{
    public partial class Barra
    {
        public int BarraId { get; set; }
        public int Idproducto { get; set; }
        public string Barcode { get; set; }

        public virtual Producto IdproductoNavigation { get; set; }
    }
}

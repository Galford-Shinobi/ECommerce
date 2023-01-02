using System;
using System.Collections.Generic;

namespace ECommerce.Common.Entities
{
    public partial class RolMenu
    {
        public int IdRolMenu { get; set; }
        public Guid? RolId { get; set; }
        public int? IdMenu { get; set; }
        public bool? EsActivo { get; set; }
        public DateTime? FechaRegistro { get; set; }

        public virtual Menu IdMenuNavigation { get; set; }
        public virtual AspNetRole Rol { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ECommerce.Common.Entities
{
    public partial class Producto
    {
        public Producto()
        {
            Barras = new HashSet<Barra>();
            BodegaProductos = new HashSet<BodegaProducto>();
        }

        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DepartamentoId { get; set; }
        public int Ivaid { get; set; }
        public decimal? Precio { get; set; }
        public string Notas { get; set; }
        public byte[] Imagen { get; set; }
        public string PathImagen { get; set; }
        public Guid? GuidImagen { get; set; }
        public int MedidaId { get; set; }
        public double Medida { get; set; }
        public decimal? Pieza { get; set; }
        public int? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual Departamento Departamento { get; set; }
        public virtual Iva Iva { get; set; }
        public virtual Medidum MedidaNavigation { get; set; }
        public virtual ICollection<Barra> Barras { get; set; }
        public virtual ICollection<BodegaProducto> BodegaProductos { get; set; }
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Common.Models.Dtos
{
    public class VMBarraProducto
    {
        public int BarraId { get; set; }
        public int Idproducto { get; set; }
        public string Barcode { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int DepartamentoId { get; set; }
        public int Ivaid { get; set; }
        public int MedidaId { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Precio { get; set; }
        public string Notas { get; set; }
        public byte[] Imagen { get; set; }
        
        public string PathImagen { get; set; }
        public Guid? GuidImagen { get; set; }
        public double Medida { get; set; }
        public decimal? Pieza { get; set; }
        public int IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public string? NombreDepartamento { get; set; }
        public string? NombreIva { get; set; }
        public string? NombreMedida { get; set; }
        [Display(Name = "Codigo Barras")]
        public string BarCodeImage { get; set; }

        [Display(Name = "Presentacion")]
        public string PictureFullPath => string.IsNullOrEmpty(PathImagen)
        ? $"http://localhost:5066/image/noimage.png"
        : string.Format("http://localhost:5066{0}", PathImagen.Substring(1));
    }
}

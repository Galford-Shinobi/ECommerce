using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Common.Models.Dtos
{
    public class VMProducto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string? CodigoBarra { get; set; }
        public string? Marca { get; set; }
        public string? Descripcion { get; set; }
        public string Notas { get; set; }
        public int MedidaId { get; set; }
        public int DepartamentoId { get; set; }
        public int Ivaid { get; set; }
        public string? NombreDepartamento { get; set; }
        public string? NombreIva { get; set; }
        public string? NombreMedida { get; set; }
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Precio { get; set; }
        public decimal? Pieza { get; set; }
        public double Medida { get; set; }
        public int EsActivo { get; set; }
        public int? Stock { get; set; }
        public string? UrlImagen { get; set; }
        public bool? HasOffer { get; set; }
        public decimal? OfferPrice { get; set; }

    }
}

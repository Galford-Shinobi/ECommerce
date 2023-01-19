using ECommerce.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ECommerce.Common.Models.Dtos
{
    public class ProductoDto
    {
        public int Idproducto { get; set; }
        
        [Display(Name = "Nombre Producto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Column(TypeName = "decimal(10,2)")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la Precio.")]
        public decimal? Precio { get; set; } = 0;
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(1550, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [DataType(DataType.MultilineText)]
        public string Notas { get; set; }
        
        public byte[] Imagen { get; set; }
        
        public string PathImagen { get; set; }
        
        public Guid? GuidImagen { get; set; }
        
        [Display(Name = "Medida")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la {0}.")]
        public int MedidaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Departamento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la  {0}.")]
        public int DepartamentoId { get; set; }
        
        [Display(Name = "Iva")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la  {0}.")]
        public int Ivaid { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Column(TypeName = "decimal(10,2)")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, double.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la  {0}.")]
        public double Medida { get; set; } = 0;

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Piezas")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1, double.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la  {0}.")]
        public decimal? Pieza { get; set; } = 0;

        public int IsActive { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Tiene Oferta")]
        public bool HasOffer { get; set; }

        [Display(Name = "Precio de oferta")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal OfferPrice { get; set; }

        public virtual ICollection<Departamento> Departamento { get; set; }

        public virtual ICollection<Iva> Iva { get; set; }

        public virtual ICollection<Medidum> MedidaNavigation { get; set; }

        public virtual ICollection<BarraDto> Barras { get; set; }

        public IEnumerable<SelectListItem> ComboDepartamentos { get; set; }
        public IEnumerable<SelectListItem> ComboIvas { get; set; }
        public IEnumerable<SelectListItem> ComboMedidas { get; set; }
        public IFormFile ImageFile { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Codigo de Barras")]
        public string Barcode { get; set; }

    }
}

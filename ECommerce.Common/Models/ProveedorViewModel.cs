using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Common.Models
{
    public class ProveedorViewModel
    {
        public int Idproveedor { get; set; }

        [Display(Name = "Nombre Proveedor")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(70, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string Nombre { get; set; }
        [Display(Name = "Tipo Documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de ingresar un valor mayor a cero en la  {0}.")]
        public int TipoDocumentoId { get; set; }
        [Display(Name = "Documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(70, MinimumLength = 15,
        ErrorMessage = "La propiedad {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        public string Documento { get; set; }

        [Display(Name = "Nombre Contacto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(70, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        [StringLength(70, MinimumLength = 5,
        ErrorMessage = "La propiedad {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        public string NombresContacto { get; set; }

        [Display(Name = "Apellido Contacto")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(70, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        [StringLength(70, MinimumLength = 5,
        ErrorMessage = "La propiedad {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        public string ApellidosContacto { get; set; }

        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        [DataType(DataType.MultilineText)]
        [StringLength(150, MinimumLength = 50,
        ErrorMessage = "La propiedad {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        public string Direccion { get; set; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string Telefono1 { get; set; }
        [Display(Name = "Celular")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(15, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        public string Telefono2 { get; set; }

        [Display(Name = "Correo Electronico")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(70, ErrorMessage = "El campo {0} debe tener una longitud máxima de {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(1550, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [DataType(DataType.MultilineText)]
        [StringLength(1550, MinimumLength = 50,
        ErrorMessage = "La propiedad {0} debe tener {1} caracteres de máximo y {2} de mínimo")]
        public string Notas { get; set; }
        public int? IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [DataType(DataType.DateTime)]
        public DateTime? RegistrationDate { get; set; }
        public IEnumerable<SelectListItem> ComboTipoDocumentos { get; set; }
    }
}

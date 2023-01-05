using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerce.Common.Models.Dtos
{
    public class GenderDto
    {
        public int GenderId { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(60, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]

        public string GeneroName { get; set; }

        [Display(Name = "descripcion del Genero")]
        [MaxLength(60, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Description { get; set; }
        public int? IsActive { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerce.Common.Models.Dtos
{
    public class ConfirmPasswordViewModel
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(65, MinimumLength = 4, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        [Display(Name = "Contraseña Actual")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        public string OldPassword { get; set; }

        [Display(Name = "Nueva Contraseña")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W]).{8,}$", ErrorMessage = "El {0} no cumple con los requisitos. Ejemplo(D*12345467a)")]
        public string NewPassword { get; set; }

        [Display(Name = "Contraseña Confirmada")]
        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W]).{8,}$", ErrorMessage = "El {0} no cumple con los requisitos. Ejemplo(D*12345467a)")]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}

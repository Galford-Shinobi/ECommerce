using System.ComponentModel.DataAnnotations;

namespace ECommerce.Common.Models.Dtos
{
    public class ResetPasswordViewModel
    {
        [MaxLength(65, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Token { get; set; }
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Jwt { get; set; }
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string UserId { get; set; }
    }
}

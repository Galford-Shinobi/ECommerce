using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerce.Common.Models.Dtos
{
    public class RecoverPasswordViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Email")]
        public string UserName { get; set; }
    }
}

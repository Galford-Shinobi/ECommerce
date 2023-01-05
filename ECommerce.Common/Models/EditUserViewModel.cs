using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerce.Common.Models
{
    public class EditUserViewModel
    {
        public Guid UserId { get; set; }
        [Display(Name = "Usurario")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string UserName { get; set; }
        [Display(Name = "Dni")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Dni { get; set; }
        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string FirstName { get; set; }
        [Display(Name = "Primer Apellido")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string SurName { get; set; }
        [Display(Name = "Segundo Apellido")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string SecondSurName { get; set; }
        [Display(Name = "Edad")]
        [MaxLength(5, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string Age { get; set; }
        [Display(Name = "Correo Electronico")]
        [MaxLength(60, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Correo electrónico normalizado")]
        [MaxLength(60, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [EmailAddress]
        public string NormalizedEmail { get; set; }
        
        [Display(Name = "Apodo")]
        [MaxLength(15, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El {0} es obligatorio")]
        public string NickName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int GenderId { get; set; }
        public string UserTimeZone { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastLoginDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? LastAccessedDate { get; set; }
        public bool? AccountLocked { get; set; }
        public int? AccessFailedCount { get; set; }
        public string PicturePath { get; set; }
        public byte[] ImagePath { get; set; }
        public int? FirstTime { get; set; }
    }
}

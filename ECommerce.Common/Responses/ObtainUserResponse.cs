using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerce.Common.Responses
{
    public class ObtainUserResponse
    {
        public Guid UserId { get; set; }
        [Display(Name = "Usuario")]
        public string UserName { get; set; }
        [Display(Name = "Dni")]
        public string Dni { get; set; }
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }
        [Display(Name = "Primer Apellido")]
        public string Surname { get; set; }
        [Display(Name = "Segundo Apellido")]
        public string SecondsurName { get; set; }
        [Display(Name = "Edad")]
        public string Age { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Apodo")]
        public string NickName { get; set; }
        [Display(Name = "Rol")]
        public string RolName { get; set; }
        [Display(Name = "Nombre Completo")]
        public string FullName => $"{FirstName} {Surname} {SecondsurName}";
        [Display(Name = "IsActive?")]
        public bool IsActive { get; set; }
        public string PicturefullPath { get; set; }
    }
}

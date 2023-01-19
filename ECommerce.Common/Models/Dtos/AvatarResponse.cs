using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Xml.Linq;

namespace ECommerce.Common.Models.Dtos
{
    public class AvatarResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Dni { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string SecondSurName { get; set; }
        public string Age { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string NickName { get; set; }
        public string RolName { get; set; }
        public string NormalizedName { get; set; }
        [Display(Name = "Logo")]
        public string PicturefullPath { get; set; }
        [Display(Name = "Nombre Completo")]
        public string FullName => $"{FirstName} {SurName} {SecondSurName}";
        [Display(Name = "IsActive?")]
        public bool IsActive { get; set; }
    }
}

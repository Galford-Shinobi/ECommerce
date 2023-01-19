using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ECommerce.Common.Models.Dtos
{
    public class UserResponseViewModel
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
        public int? IsActive { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? RegistrationDate { get; set; }

        public string Password { get; set; }

        [Display(Name = "Logo")]
        public string PictureFullPath
        {
            get
            {
                if (PicturePath == null)
                {
                    return "http://localhost:18517//img/noimage.png";
                }

                return string.Format(
                    "http://localhost:18517/{0}",
                    PicturePath.Substring(1));
            }
        }
    }
}

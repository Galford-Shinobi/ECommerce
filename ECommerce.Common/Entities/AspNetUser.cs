using System;
using System.Collections.Generic;

namespace ECommerce.Common.Entities
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            HistorialRefreshTokens = new HashSet<HistorialRefreshToken>();
        }

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
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastAccessedDate { get; set; }
        public bool? AccountLocked { get; set; }
        public int? AccessFailedCount { get; set; }
        public string PicturePath { get; set; }
        public byte[] ImagePath { get; set; }
        public int? FirstTime { get; set; }
        public int? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public virtual Genero Gender { get; set; }
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<HistorialRefreshToken> HistorialRefreshTokens { get; set; }
    }
}

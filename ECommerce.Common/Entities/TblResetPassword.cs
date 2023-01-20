using System;
using System.Collections.Generic;

namespace ECommerce.Common.Entities
{
    public partial class TblResetPassword
    {
        public Guid ResetPasswordId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Jwt { get; set; }
        public string Token { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? IsDeleted { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}

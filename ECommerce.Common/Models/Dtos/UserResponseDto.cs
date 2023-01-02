namespace ECommerce.Common.Models.Dtos
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public Guid RolId { get; set; }
        public string UserName { get; set; }
        public string Dni { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string SecondsurName { get; set; }
        public string Age { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string RolName { get; set; }
        public string FullName => $"{FirstName} {Surname} {SecondsurName}";
    }
}

namespace ECommerce.Common.Responses
{
    public class UserResponse
    {
        public Guid UserId { get; set; }
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

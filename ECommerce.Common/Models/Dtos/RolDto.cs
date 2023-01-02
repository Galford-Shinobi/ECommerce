namespace ECommerce.Common.Models.Dtos
{
    public class RolDto
    {
        public Guid RolId { get; set; }
        public string Rnombre { get; set; }
        public string NormalizedName { get; set; }
        public int? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}

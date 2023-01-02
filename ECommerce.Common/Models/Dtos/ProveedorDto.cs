namespace ECommerce.Common.Models.Dtos
{
    public class ProveedorDto
    {
        public int Idproveedor { get; set; }
        public string Nombre { get; set; }
        public int TipoDocumentoId { get; set; }
        public string Documento { get; set; }
        public string NombresContacto { get; set; }
        public string ApellidosContacto { get; set; }
        public string Direccion { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Correo { get; set; }
        public string Notas { get; set; }
        public string DocumentoTipo { get; set; }
        public int? IsActive { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}

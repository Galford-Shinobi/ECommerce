using ECommerce.App.Helpers.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models;

namespace ECommerce.App.Helpers.Repositories
{
    public class ConverterHelper : IConverterHelper
    {
        public Proveedor ToProveedorsAsync(ProveedorViewModel model, bool isNew)
        {
            return new Proveedor
            {
                Idproveedor = isNew ? 0 : model.Idproveedor,
                Nombre = model.Nombre,
                Correo = model.Correo,
                Direccion = model.Direccion,
                Documento = model.Documento,
                NombresContacto = model.NombresContacto,
                ApellidosContacto = model.ApellidosContacto,
                Notas = model.Notas,
                Telefono1 = model.Telefono1,
                Telefono2 = model.Telefono2,
                IsActive= model.IsActive,
                TipoDocumentoId = model.TipoDocumentoId,
                RegistrationDate = DateTime.Now.ToUniversalTime(),
            };
        }
    }
}

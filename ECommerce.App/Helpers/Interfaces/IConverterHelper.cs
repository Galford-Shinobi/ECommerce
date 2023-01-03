using ECommerce.Common.Entities;
using ECommerce.Common.Models;

namespace ECommerce.App.Helpers.Interfaces
{
    public interface IConverterHelper
    {
        Proveedor ToProveedorsAsync(ProveedorViewModel model, bool isNew);
    }
}

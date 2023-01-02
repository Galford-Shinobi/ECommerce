using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;

namespace ECommerce.Common.Application.Interfaces
{
    public interface IProveedorRepository : IGenericRepositoryFactory<Proveedor>
    {
        Task<List<ProveedorDto>> GetAllProveedorAsync();
        Task<GenericResponse<ProveedorDto>> GetOnlyProveedorAsync(int id);
        Task<GenericResponse<Proveedor>> OnlyProveedorGetAsync(int id);
        Task<GenericResponse<Proveedor>> DeleteProveedorAsync(int id);
        Task<GenericResponse<ProveedorDto>> DeactivateProveedorAsync(ProveedorDto avatar);
    }
}

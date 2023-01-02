using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;

namespace ECommerce.Common.Application.Interfaces
{
    public interface IRolRepository : IGenericRepositoryFactory<AspNetRole>
    {
        Task<List<RolDto>> GetAllRolesAsync();
        Task<GenericResponse<RolDto>> GetOnlyRolAsync(Guid id);
        Task<GenericResponse<AspNetRole>> OnlyRolGetAsync(Guid id);
        Task<GenericResponse<AspNetRole>> DeleteRolAsync(Guid id);
        Task<GenericResponse<RolDto>> DeactivateRolAsync(RolDto avatar);
    }
}

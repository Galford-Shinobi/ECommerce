using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;

namespace ECommerce.Common.Application.Interfaces
{
    public interface IGenderRepository : IGenericRepositoryFactory<Genero>
    {
        Task<List<GenderDto>> GetAllGenderAsync();
        Task<GenericResponse<GenderDto>> GetOnlyGenderAsync(int id);
        Task<GenericResponse<Genero>> OnlyGenderGetAsync(int id);
        Task<GenericResponse<Genero>> DeleteGenderAsync(int id);
        Task<GenericResponse<GenderDto>> DeactivateGenderAsync(GenderDto avatar);
    }
}

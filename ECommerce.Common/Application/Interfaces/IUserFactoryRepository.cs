using ECommerce.Common.Entities;
using ECommerce.Common.Models;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;

namespace ECommerce.Common.Application.Interfaces
{
    public interface IUserFactoryRepository : IGenericRepositoryFactory<AspNetUser>
    {
        Task<GenericResponse<UserResponseDto>> LoginAsync(LoginViewModel model);
        Task<GenericResponse<object>> AccessFailedCountAsync(LoginViewModel model);
        TokenResponse GetToken(string UserName);
        string GenerateJWTToken(UserResponse userInfo);
    }
}

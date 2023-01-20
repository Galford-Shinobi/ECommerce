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
        Task<GenericResponse<AvatarResponse>> GetUserByEmailAsync(string UserName);
        Task<GenericResponse<AvatarResponse>> GetConfirmPasswordAsync(ConfirmPasswordViewModel model);
        Task<GenericResponse<TokenResponse>> GeneratePasswordResetTokenAsync(RecoverPasswordViewModel user);
        Task<GenericResponse<ObtainUserResponse>> GetToObtainUserAsync(Guid UserId, string UserName);
        Task<GenericResponse<object>> TBResetPasswordsAsync(TblResetPassword model);
        Task<GenericResponse<object>> ResetPasswordAsync(ObtainUserResponse model, string Password, string jwt, string token, string password);
    }
}

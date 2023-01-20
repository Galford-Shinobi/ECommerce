using AutoMapper;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.DataBase;
using ECommerce.Common.Entities;
using ECommerce.Common.Models;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.Common.Application.Implementacion
{
    public class UserFactoryRepository : GenericRepository<AspNetUser>, IUserFactoryRepository
    {
        private readonly ECommerceDbContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserFactoryRepository(ECommerceDbContext dbContext, IMapper mapper, IConfiguration Configuration) : base(dbContext)
        {
            _dataContext = dbContext;
            _mapper = mapper;
            _configuration = Configuration;
        }

        private bool VerificaPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<GenericResponse<UserResponseDto>> LoginAsync(LoginViewModel model)
        {
            try
            {
                AspNetUser user = await _dataContext.AspNetUsers
                .FirstOrDefaultAsync(x => x.Email == model.Username);

                if (user == null)
                {
                    return new GenericResponse<UserResponseDto>
                    {
                        IsSuccess = false,
                        Message = "wrong username or password",
                        TruePasswordHash = -1,
                    };
                }


                if (user.AccountLocked == true || user.AccessFailedCount >= 3)
                {
                    await AccessFailedCountAsync(model);
                    return new GenericResponse<UserResponseDto>
                    {
                        IsSuccess = false,
                        Message = "Your Access is compromised(Check with your Administrator, thanks)",
                        TruePasswordHash = -5,
                    };
                }

                if (user.FirstTime == 1)
                {
                    await AccessFailedCountAsync(model);
                    return new GenericResponse<UserResponseDto>
                    {
                        IsSuccess = false,
                        Message = "has not made the confirmation in the system",
                        TruePasswordHash = -10,
                    };
                }

                if (user.IsActive != 1)
                {
                    await AccessFailedCountAsync(model);
                    return new GenericResponse<UserResponseDto>
                    {
                        IsSuccess = false,
                        Message = "has not made the confirmation in the system",
                        TruePasswordHash = -30,
                    };
                }




                if (!VerificaPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    await AccessFailedCountAsync(model);
                    return new GenericResponse<UserResponseDto>
                    {
                        IsSuccess = false,
                        Message = "wrong username or password",
                        TruePasswordHash = -20,
                    };
                }

                var NetRolUser = await _dataContext.AspNetUserRoles
                    .Include(u => u.Rol)
                    .Include(u => u.User)
                    .FirstOrDefaultAsync(u => u.User.UserName.Equals(model.Username) && u.User.UserId.Equals(user.UserId) && u.User.IsActive == 1);

                if (NetRolUser == null)
                {
                    return new GenericResponse<UserResponseDto>
                    {
                        IsSuccess = false,
                        Message = "wrong username or password",
                        TruePasswordHash = -03,
                    };
                }

                UserResponseDto userResponse = new UserResponseDto
                {
                    UserId = user.UserId,
                    UserName = NetRolUser.User.UserName,
                    FirstName = NetRolUser.User.FirstName,
                    Surname = NetRolUser.User.SurName,
                    SecondsurName = NetRolUser.User.SecondSurName,
                    RolName = NetRolUser.Rol.Rnombre,
                    RolId = NetRolUser.Rol.RolId,
                    Email = NetRolUser.User.Email,
                    NickName = NetRolUser.User.NickName,
                    Dni = NetRolUser.User.Dni,
                };

                user.LastAccessedDate = DateTime.Now.ToUniversalTime();
                user.LastLoginDate = DateTime.Now.ToUniversalTime();
                _dataContext.AspNetUsers.Update(user);
                await _dataContext.SaveChangesAsync();
                return new GenericResponse<UserResponseDto>
                {
                    IsSuccess = true,
                    Message = "Congratulations, you can continue with your activities.",
                    Result = userResponse,
                    TruePasswordHash = 0,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<UserResponseDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<GenericResponse<object>> AccessFailedCountAsync(LoginViewModel model)
        {
            try
            {
                var _User = await _dataContext.AspNetUsers
                    .FirstOrDefaultAsync(u => u.UserName.Equals(model.Username));

                if (_User == null)
                {
                    return new GenericResponse<object>
                    {
                        IsSuccess = false,
                        Message = "Alert the system found an inconsistency consult your Administrator",
                    };
                }
                if (_User.AccessFailedCount == 0)
                {
                    _User.AccessFailedCount = 1;
                }
                else
                {
                    _User.AccessFailedCount = _User.AccessFailedCount++;
                }
                _User.LastAccessedDate = DateTime.Now.ToUniversalTime();
                _User.LastLoginDate = DateTime.Now.ToUniversalTime();
                _dataContext.AspNetUsers.Update(_User);
                await _dataContext.SaveChangesAsync();
                return new GenericResponse<object>
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<object>
                {
                    IsSuccess = false,
                    Message = $"Alert the system found an inconsistency consult your Administrator {ex.Message}",
                };
            }
        }

        public TokenResponse GetToken(string UserName)
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:SecretKey"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials);
            return new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }

        public string GenerateJWTToken(UserResponse userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //List<Claim> claims = new List<Claim>() {
            //    new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
            //    new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
            //    new Claim(ClaimTypes.Role, usuario_encontrado.IdRol.ToString()),
            //    new Claim("UrlFoto", usuario_encontrado.UrlFoto),
            //};

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Email),
                new Claim("fullName", $"{userInfo.FirstName.ToString()}{" "}{userInfo.Surname.ToString()}{" "}{userInfo.SecondsurName}"),
                new Claim("FolioNumber",userInfo.UserId.ToString()),
                 new Claim("Email", userInfo.UserName),
                new Claim("Role", userInfo.RolName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Tokens:Issuer"],
                audience: _configuration["Tokens:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<GenericResponse<AvatarResponse>> GetUserByEmailAsync(string UserName)
        {
            try
            {
                AvatarResponse avatarResponse = new AvatarResponse();
                var user = await _dataContext
                    .AspNetUserRoles
                    .Include(r => r.Rol)
                    .Include(u => u.User)
                    .FirstOrDefaultAsync(u => u.User.Email.Equals(UserName));
                if (user == null)
                {
                    return new GenericResponse<AvatarResponse>
                    {
                        IsSuccess = false,
                        Message = "Error 2019: Se ha producido un error en uno de los elementos principales. Corrige los errores o consulte a su Administrador!."
                    };
                }

                avatarResponse.FirstName = user.User.FirstName;
                avatarResponse.SurName = user.User.SurName;
                avatarResponse.SecondSurName = user.User.SecondSurName;
                avatarResponse.Age = user.User.Age;
                avatarResponse.Dni = user.User.Dni;
                avatarResponse.Email = user.User.Email;
                avatarResponse.UserId = user.User.UserId;
                avatarResponse.NickName = user.User.NickName;
                avatarResponse.UserName = user.User.UserName;
                avatarResponse.PicturefullPath = user.User.PictureFullPath;
                avatarResponse.NormalizedName = user.Rol.NormalizedName.ToString();
                avatarResponse.RolName = user.Rol.Rnombre;

                return new GenericResponse<AvatarResponse>() { 
                    IsSuccess= true,
                    Result= avatarResponse
                };
            }
            catch (Exception exception)
            {
                return new GenericResponse<AvatarResponse>() { 
                 IsSuccess= false,
                 ErrorMessage= exception.Message    
                };
            }
        }
        public async Task<GenericResponse<AvatarResponse>> GetConfirmPasswordAsync(ConfirmPasswordViewModel model)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    var Secret = _configuration["SecretP:SecretPassword"];

                    if (Secret != model.OldPassword)
                    {
                        return new GenericResponse<AvatarResponse>
                        {
                            IsSuccess = false,
                            Message = "wrong username or password",
                            TruePasswordHash = -21,
                        };
                    }
                    var _viewUser = await _dataContext
                        .AspNetUsers
                        .FirstOrDefaultAsync(u => u.Email.Equals(model.UserName) && u.FirstTime == 1);

                    if (_viewUser == null)
                    {
                        return new GenericResponse<AvatarResponse>
                        {
                            IsSuccess = false,
                            Message = "wrong username or password",
                            TruePasswordHash = -22,
                        };
                    }

                    if (!VerificaPasswordHash(model.OldPassword, _viewUser.PasswordHash, _viewUser.PasswordSalt))
                    {
                        return new GenericResponse<AvatarResponse>
                        {
                            IsSuccess = false,
                            Message = "wrong username or password",
                            TruePasswordHash = -23,
                        };
                    }
                    byte[] passwordHash, passwordSalt;
                    CrearPasswordHash(model.NewPassword, out passwordHash, out passwordSalt);
                    _viewUser.IsActive = 1;
                    _viewUser.FirstTime = 0;
                    _viewUser.RegistrationDate = DateTime.Now.ToUniversalTime();
                    _viewUser.PasswordHash = passwordHash;
                    _viewUser.PasswordSalt = passwordSalt;
                    _dataContext.AspNetUsers.Update(_viewUser);

                    await _dataContext.SaveChangesAsync();
                    transaction.Commit();
                    return new GenericResponse<AvatarResponse>
                    {
                        IsSuccess = true,
                        Message = "operation carried out successfully",
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new GenericResponse<AvatarResponse>
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                    };
                }
            }
        }

        public async Task<GenericResponse<TokenResponse>> GeneratePasswordResetTokenAsync(RecoverPasswordViewModel user)
        {
            try
            {
                var _NetUsers = await _dataContext
                    .AspNetUsers
                    .Where(u => u.UserName.ToUpper() == user.UserName.ToUpper() && u.IsActive == 1)
                    .FirstOrDefaultAsync();


                if (_NetUsers == null)
                {
                    return new GenericResponse<TokenResponse>
                    {
                        IsSuccess = false,
                        Message = "requested information was not found",
                    };
                }

                var tokenr = GetToken(user.UserName);
                var avalon = await GetToObtainUserAsync(_NetUsers.UserId, _NetUsers.Email);
                tokenr.ObtainUser = avalon.Result;
                return new GenericResponse<TokenResponse>
                {
                    IsSuccess = true,
                    Result = tokenr,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<TokenResponse>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<GenericResponse<ObtainUserResponse>> GetToObtainUserAsync(Guid UserId, string UserName)
        {
            try
            {
                ObtainUserResponse ObtainUser = await (from ur in _dataContext.AspNetUserRoles
                                                       join r in _dataContext.AspNetRoles
                                                       on ur.RolId equals r.RolId
                                                       join u in _dataContext.AspNetUsers
                                                       on ur.UserId equals u.UserId
                                                       where u.UserId.Equals(UserId) || u.Email.Equals(UserName)
                                                       select new ObtainUserResponse
                                                       {

                                                           Age = u.Age,
                                                           Dni = u.Dni,
                                                           UserId = u.UserId,
                                                           Email = u.Email,
                                                           FirstName = u.FirstName,
                                                           Surname = u.SurName,
                                                           SecondsurName = u.SecondSurName,
                                                           NickName = u.NickName,
                                                           PicturefullPath = u.PictureFullPath,
                                                           UserName = u.UserName,
                                                           RolName = r.Rnombre,
                                                           IsActive = u.IsActive == 1 ? true : false,
                                                       }
                          ).FirstOrDefaultAsync();
                return new GenericResponse<ObtainUserResponse>
                {
                    IsSuccess = true,
                    Result = ObtainUser,
                };
            }
            catch (Exception ex)
            {

                return new GenericResponse<ObtainUserResponse>
                {
                    IsSuccess = false,
                    Message = ex.InnerException.Message,
                };
            }
        }

        public async Task<GenericResponse<object>> ResetPasswordAsync(ObtainUserResponse model, string Password, string jwt, string token, string password)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    //var _Tbl = await _dataContext
                    //    .TblResetPassword
                    //    .FirstOrDefaultAsync(x => x.Jwt.Equals(jwt) && x.Token.Equals(token) && x.UserName.Equals(model.UserName) && x.UserId.Equals(model.UserId) && x.IsDeleted.Equals(10));
                    //if (_Tbl == null)
                    //{
                    //    return new Response<object>
                    //    {
                    //        IsSuccess = false,
                    //        Message = "Do not Data!",
                    //    };
                    //}

                    //var _AspNetUsers = await _dataContext.TblAspNetUsers
                    //   .Where(u => u.NUser.ToUpper() == model.UserName.ToUpper() && u.Status == 1 && u.UserId.Equals(model.UserId))
                    //   .FirstOrDefaultAsync();

                    //if (_AspNetUsers == null)
                    //{
                    //    return new Response<object>
                    //    {
                    //        IsSuccess = false,
                    //        Message = "the user data is not correct check the data ....!"
                    //    };
                    //}
                    //byte[] passwordHash, passwordSalt;
                    //CrearPasswordHash(Password, out passwordHash, out passwordSalt);
                    //_AspNetUsers.PasswordHash = passwordHash;
                    //_AspNetUsers.PasswordSalt = passwordSalt;

                    //_dataContext.TblAspNetUsers.Update(_AspNetUsers);


                    //_dataContext.TblResetPassword.Remove(_Tbl);
                    //await _dataContext.SaveChangesAsync();
                    //transaction.Commit();
                    return new GenericResponse<object>
                    {
                        IsSuccess = true,
                        Message = "Win!",
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new GenericResponse<object>
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                    };
                }
            }
        }
        public async Task<GenericResponse<object>> TBResetPasswordsAsync(TblResetPassword model)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _dataContext.Database.BeginTransaction()) 
            {
                try
                {
                    _dataContext.TblResetPasswords.Add(model);

                    var historyUpdate = new HistorialRefreshToken() {
                        EsActivo = true,
                        FechaCreacion = DateTime.Now.ToUniversalTime(),
                        FechaExpiracion =  model.ExpirationDate,
                        RefreshToken = model.Jwt,
                        Token = model.Token,
                        UserId = model.UserId,
                    };

                    _dataContext.HistorialRefreshTokens.Add(historyUpdate);
                    await _dataContext.SaveChangesAsync();

                   await transaction.CommitAsync();
                    return new GenericResponse<object>
                    {
                        IsSuccess = true,
                    };
                }
                catch (DbUpdateException dbUpdateException)
                {
                   await transaction.RollbackAsync();
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {

                        return new GenericResponse<object>
                        {
                            IsSuccess = false,
                            ErrorMessage = $"Error 1622: Palabras clave duplicadas encontradas bajo la misma entidad principal. Elimina todos los duplicados.({model.UserName})."
                        };
                    }
                    else
                    {

                        return new GenericResponse<object>
                        {
                            IsSuccess = false,
                            ErrorMessage = dbUpdateException.InnerException.Message
                        };
                    }
                }
                catch (Exception ex)
                {
                   await transaction.RollbackAsync();
                    return new GenericResponse<object>
                    {
                        IsSuccess = false,
                        ErrorMessage = ex.InnerException.Message,
                    };
                }
            }
        }
    }
}

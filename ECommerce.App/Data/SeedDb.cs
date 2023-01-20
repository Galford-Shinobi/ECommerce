using Microsoft.EntityFrameworkCore;
using ECommerce.Common.DataBase;
using ECommerce.Common.Entities;
using ECommerce.Common.Enums;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;

namespace ECommerce.App.Data
{
    public class SeedDb
    {
        private readonly ECommerceDbContext __dataContext;

        public SeedDb(ECommerceDbContext dataContext)
        {
            __dataContext = dataContext;
        }
        public async Task SeedAsync() {
            await __dataContext.Database.EnsureCreatedAsync();
            await CheckConceptoAsync();
            await CheckBodegasAsync();
            await CheckDepartamentosAsync();
            await CheckIvaAsync();
            await CheckMedidumAsync();
            await CheckTipoDocumentosAsync();
            await CheckGeneroAsync();
            await CheckRolesAsync();
            await CheckGendersAsync();
            await CheckAdminsAsync();
        }


        private async Task CheckGendersAsync()
        {
            if (!__dataContext.Generos.Any())
            {
                __dataContext.Generos.Add(new Genero { GeneroName= "Femenino", Description = "F", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Generos.Add(new Genero { GeneroName = "Masculino", Description = "M", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Generos.Add(new Genero { GeneroName = "Otr@s", Description = "OT", IsActive = 1, RegistrationDate = DateTime.UtcNow });
               
                await __dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckConceptoAsync()
        {
            if (!__dataContext.Conceptos.Any())
            {
                __dataContext.Conceptos.Add(new Concepto { Descripcion = "Averia", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Conceptos.Add(new Concepto { Descripcion = "Autoconsumo", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Conceptos.Add(new Concepto { Descripcion = "Hurto", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Conceptos.Add(new Concepto { Descripcion = "Donación", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                
                await __dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckTipoDocumentosAsync()
        {
            if (!__dataContext.TipoDocumentos.Any())
            {
                __dataContext.TipoDocumentos.Add(new TipoDocumento { Descripcion = "INE", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.TipoDocumentos.Add(new TipoDocumento { Descripcion = "Cédula de Ciudadanía", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.TipoDocumentos.Add(new TipoDocumento { Descripcion = "NIT", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.TipoDocumentos.Add(new TipoDocumento { Descripcion = "Tarjeta de Identidad", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.TipoDocumentos.Add(new TipoDocumento { Descripcion = "Cédula de Extranjería", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.TipoDocumentos.Add(new TipoDocumento { Descripcion = "Cédula de Alienígena", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                
                await __dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckGeneroAsync()
        {
            if (!__dataContext.Generos.Any())
            {
                __dataContext.Generos.Add(new Genero { GeneroName="Masculino" ,Description = "M", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Generos.Add(new Genero { GeneroName = "Femenino", Description = "F", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Generos.Add(new Genero { GeneroName = "Rayito", Description = "LGTHJK", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Generos.Add(new Genero {GeneroName = "Otr@s",Description = "O", IsActive = 1, RegistrationDate = DateTime.UtcNow });

                await __dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckBodegasAsync()
        {
            if (!__dataContext.Bodegas.Any())
            {
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "Principal", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "Envigado", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "Itagüí", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "Sabaneta", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "Medellín", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "Bello", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "Cocorna", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "Puerto Berrio de los Dolores", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Bodegas.Add(new Bodega { Descripcion = "La Estrella", IsActive = 1, RegistrationDate = DateTime.UtcNow });



                await __dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckDepartamentosAsync()
        {
            if (!__dataContext.Departamentos.Any())
            {
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Licores", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Aseo Personal", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Aseo Hogar", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Ferretería", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Niños y Niñas", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Interior Masculino", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Interior Femenino", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Frutas y Verduras", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Granos", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Electrodomésticos", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Farmacia", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Panadería", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Belleza Mujer", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Jugos Naturales", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Deporte", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Literatura", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Arte", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Lacteos", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Seguridad Personal", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Charcutería", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Carnicería", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Departamentos.Add(new Departamento { Descripcion = "Salsas", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                
                await __dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckIvaAsync()
        {
            if (!__dataContext.Ivas.Any())
            {
                __dataContext.Ivas.Add(new Iva { Descripcion = "Excluido", Tarifa = 0.0M, IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Ivas.Add(new Iva { Descripcion = "Exento", Tarifa = 0.0M, IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Ivas.Add(new Iva { Descripcion = "IVA 10%", Tarifa = 0.10M, IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Ivas.Add(new Iva { Descripcion = "IVA 16%", Tarifa = 0.16M, IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Ivas.Add(new Iva { Descripcion = "IVA 20%", Tarifa = 0.20M, IsActive = 1, RegistrationDate = DateTime.UtcNow });

                await __dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckMedidumAsync()
        {
            if (!__dataContext.Medida.Any())
            {
                __dataContext.Medida.Add(new Medidum { Descripcion = "Gramos", Escala = "GR", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Medida.Add(new Medidum { Descripcion = "Kilogramo", Escala = "KG", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Medida.Add(new Medidum { Descripcion = "Litro", Escala = "LT", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Medida.Add(new Medidum { Descripcion = "Metro", Escala = "MT", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Medida.Add(new Medidum { Descripcion = "Onza", Escala = "OZ", IsActive = 1, RegistrationDate = DateTime.UtcNow });
                __dataContext.Medida.Add(new Medidum { Descripcion = "Unidad", Escala = "UN", IsActive = 1, RegistrationDate = DateTime.UtcNow });

                await __dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckRolesAsync()
        {
            if (!__dataContext.AspNetRoles.Any())
            {
                var _rol = new List<AspNetRole>{
                            new AspNetRole {RolId = Guid.NewGuid() , Rnombre  = "Administrator",     NormalizedName = "ADIMINSTRATOR",    IsActive =1 , RegistrationDate = DateTime.Now.ToUniversalTime() },
                            new AspNetRole {RolId = Guid.NewGuid() , Rnombre  = "SuperUser",      NormalizedName = "SUPERUSER",      IsActive =1 , RegistrationDate = DateTime.Now.ToUniversalTime() },
                            new AspNetRole {RolId = Guid.NewGuid() , Rnombre  = "User",     NormalizedName = "USER",     IsActive =1 , RegistrationDate = DateTime.Now.ToUniversalTime() },

                };

                __dataContext.AspNetRoles.AddRange(_rol);
                await __dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckAdminsAsync()
        {
            var userModel = new List<UserResponseViewModel> { new UserResponseViewModel { UserName = "uhtred.bebbanburg@yopmail.com",
                Dni = "000001",
                FirstName = "Quinlan Vos Shadow",
                SurName = "Dark",
                SecondSurName = "Disciple",
                Email = "dark.disciple@yopmail.com",
                Age = "500",
                NickName = "God",
                PicturePath = $"{"~/image/QuinlanVos.png"}",
                Password = "User*123456", },};

          await  CheckUserAsync(userModel, UserType.SuperUser.ToString());
        }
        private static void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private async Task<GenericResponse<UserResponse>> CheckUserAsync(List<UserResponseViewModel> model, string _Rol)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = __dataContext.Database.BeginTransaction())
            {
                try
                {
                    UserResponse user = new UserResponse();
                    foreach (var item in model.ToList())
                    {
                        var _register = await __dataContext.AspNetUsers
                        .Where(u => u.Email.Equals(item.Email) || u.UserName == item.UserName)
                        .FirstOrDefaultAsync();
                        if (_register != null)
                        {
                            return new GenericResponse<UserResponse>
                            {
                                IsSuccess = false,
                                Message = "hay información sobre la aplicación. Registrarse",
                            };
                        }

                        AspNetRole _roles = await __dataContext
                            .AspNetRoles
                            .FirstOrDefaultAsync(r => r.Rnombre.Equals(_Rol) && r.IsActive == 1);

                        if (_roles == null)
                        {
                            return new GenericResponse<UserResponse>
                            {
                                IsSuccess = false,
                                Message = "No hay información sobre la aplicación. Registro Romeo",
                            };
                        }
                        AspNetUser _users = new AspNetUser
                        {
                            UserId = Guid.NewGuid(),
                            Dni = item.Dni,
                            FirstName = item.FirstName,
                            SurName = item.SurName,
                            SecondSurName = item.SecondSurName,
                            GenderId = 2,
                            Email = item.Email,
                            NormalizedEmail = item.Email.ToUpper(),
                            UserName = item.Email,
                            Age = item.Age,
                            NickName = item.NickName,
                            PicturePath = string.IsNullOrEmpty(item.PicturePath) ? null : item.PicturePath,
                            FirstTime = 0,
                            IsActive = 1,
                            AccessFailedCount = 0,
                            RegistrationDate = DateTime.Now.ToUniversalTime(),
                        };
                        byte[] passwordHash, passwordSalt;
                        CrearPasswordHash(item.Password, out passwordHash, out passwordSalt);

                        _users.PasswordHash = passwordHash;
                        _users.PasswordSalt = passwordSalt;

                        __dataContext.AspNetUsers.Add(_users);
                        await __dataContext.SaveChangesAsync();

                        AspNetUserRole UsersRols = __dataContext
                        .AspNetUserRoles
                        .Where(r => r.RolId == _roles.RolId && r.UserId == _users.UserId)
                        .FirstOrDefault();

                        if (UsersRols != null)
                        {
                            return new GenericResponse<UserResponse>
                            {
                                IsSuccess = false,
                                Message = "No hay información sobre la aplicación.",
                            };
                        }
                        AspNetUserRole usersRole = new AspNetUserRole
                        {
                            RolId = _roles.RolId,
                            UserId = _users.UserId,
                        };
                        __dataContext.AspNetUserRoles.Add(usersRole);
                        await __dataContext.SaveChangesAsync();


                        user.FirstName = _users.FirstName;
                        user.UserId = _users.UserId;
                        user.Email = _users.Email;
                        user.UserName = _users.UserName;
                        user.Dni = _users.Dni;
                        user.Surname = _users.SurName;
                        user.SecondsurName = _users.SecondSurName;
                        user.Age = _users.Age;
                        user.NickName = _users.NickName;
                    }
                    transaction.Commit();
                    return new GenericResponse<UserResponse>
                    {
                        IsSuccess = true,
                        Message = "The information has already been registered in the system.",
                        Result = user,
                    };
                }
                catch (DbUpdateException dbUpdateException)
                {
                    transaction.Rollback();
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        return new GenericResponse<UserResponse>
                        {
                            IsSuccess = false,
                            ErrorMessage = dbUpdateException.InnerException.Message,
                        };
                    }
                    else
                    {
                        return new GenericResponse<UserResponse>
                        {
                            IsSuccess = false,
                            Message = dbUpdateException.InnerException.Message,
                        };
                    }
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    return new GenericResponse<UserResponse>
                    {
                        IsSuccess = false,
                        Message = exception.InnerException.Message,
                    };
                }
            }
        }
    }
}

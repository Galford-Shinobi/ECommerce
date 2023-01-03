using AutoMapper;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.DataBase;
using ECommerce.Common.Entities;
using ECommerce.Common.Models;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Common.Application.Implementacion
{
    public class ProveedorRepository : GenericRepository<Proveedor>, IProveedorRepository
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProveedorRepository(ECommerceDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GenericResponse<ProveedorDto>> DeactivateProveedorAsync(ProveedorDto avatar)
        {
            try
            {
                var OnlyProvee = await _dbContext
                    .Proveedors.FirstOrDefaultAsync(c => c.IDProveedor == avatar.Idproveedor);
                OnlyProvee.IsActive = 0;
                _dbContext.Proveedors.Update(OnlyProvee);
                await SaveAllAsync();
                return new GenericResponse<ProveedorDto> { IsSuccess = true, Result = avatar };

            }
            catch (Exception ex)
            {
                return new GenericResponse<ProveedorDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<Proveedor>> DeleteProveedorAsync(int id)
        {
            try
            {
                var proveedor = await _dbContext.Proveedors.FirstOrDefaultAsync(c => c.IDProveedor == id);
                if (proveedor == null)
                {
                    return new GenericResponse<Proveedor> { IsSuccess = false, Message = "No hay Datos!" };
                }

                proveedor.IsActive = 0;

                _dbContext.Proveedors.Update(proveedor);
                if (!await SaveAllAsync())
                {
                    return new GenericResponse<Proveedor> { IsSuccess = false, Message = "La operacion no realizada!" };
                }

                return new GenericResponse<Proveedor> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new GenericResponse<Proveedor> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<List<ProveedorDto>> GetAllProveedorAsync()
        {
            var listAll = await _dbContext.Proveedors.Include(t => t.TipoDocumento).Where(c => c.IsActive == 1).OrderBy(c => c.IDProveedor).ToListAsync();
            var ListDto = new List<ProveedorDto>();

            foreach (var list in listAll)
            {
                ListDto.Add(_mapper.Map<ProveedorDto>(list));
            }
            return ListDto;
        }

        public async Task<GenericResponse<ProveedorDto>> GetOnlyProveedorAsync(int id)
        {
            try
            {
                var Only = await _dbContext.Proveedors.FirstOrDefaultAsync(c => c.IDProveedor.Equals(id));
                if (Only == null)
                {
                    return new GenericResponse<ProveedorDto> { IsSuccess = false, Message = "No hay Datos!" };
                }
                var OnlyConcepto = _mapper.Map<ProveedorDto>(Only);
                return new GenericResponse<ProveedorDto> { IsSuccess = true, Result = OnlyConcepto };

            }
            catch (Exception ex)
            {
                return new GenericResponse<ProveedorDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<Proveedor>> OnlyProveedorGetAsync(int id)
        {
            try
            {
                var Onlyproveedor = await _dbContext.Proveedors.FirstOrDefaultAsync(c => c.IDProveedor.Equals(id));
                if (Onlyproveedor == null)
                {
                    return new GenericResponse<Proveedor> { IsSuccess = false, Message = "No hay Datos!" };
                }

                return new GenericResponse<Proveedor> { IsSuccess = true, Result = Onlyproveedor };

            }
            catch (Exception ex)
            {
                return new GenericResponse<Proveedor> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<Proveedor>> OnlyUpDateAsync(ProveedorViewModel model)
        {
            try
            {
                var OnlyProv = await _dbContext.Proveedors.FirstOrDefaultAsync(p => p.IDProveedor == model.Idproveedor);

                if (OnlyProv==null)
                {
                    return new GenericResponse<Proveedor>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"registro no existe en el sistema verifique los datos!   {model.Nombre}",
                    };
                }

                OnlyProv.Nombre = model.Nombre ?? OnlyProv.Nombre;
                OnlyProv.NombresContacto = model.NombresContacto ?? OnlyProv.NombresContacto;
                OnlyProv.ApellidosContacto = model.ApellidosContacto ?? OnlyProv.ApellidosContacto;
                OnlyProv.Notas = model.Notas ?? OnlyProv.Notas;
                OnlyProv.Documento = model.Documento ?? OnlyProv.Documento;
                OnlyProv.Correo = model.Correo ?? OnlyProv.Correo;
                OnlyProv.Direccion = model.Direccion ?? OnlyProv.Direccion;
                OnlyProv.Telefono1 = model.Telefono1 ?? OnlyProv.Telefono1;
                OnlyProv.Telefono2 = model.Telefono2 ?? OnlyProv.Telefono2;
                OnlyProv.TipoDocumentoId = model.TipoDocumentoId;
                _dbContext.Proveedors.Update(OnlyProv);
               await _dbContext.SaveChangesAsync();
                return new GenericResponse<Proveedor>
                {
                    IsSuccess = true,
                    Message = "win!",
                };
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return new GenericResponse<Proveedor>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Ya existe en el sistema verifique los datos!   {model.Nombre}",
                    };
                }
                else
                {
                    return new GenericResponse<Proveedor>
                    {
                        IsSuccess = false,
                        ErrorMessage = dbUpdateException.InnerException.Message,
                    };
                }
            }
            catch (Exception exception)
            {
                return new GenericResponse<Proveedor>
                {
                    IsSuccess = false,
                    ErrorMessage = exception.InnerException.Message,
                };
            }
        }

        private async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}

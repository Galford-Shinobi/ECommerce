using AutoMapper;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.DataBase;
using ECommerce.Common.Entities;
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
                    .Proveedors.FirstOrDefaultAsync(c => c.Idproveedor == avatar.Idproveedor);
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
                var proveedor = await _dbContext.Proveedors.FirstOrDefaultAsync(c => c.Idproveedor == id);
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
            var listAll = await _dbContext.Proveedors.Include(t => t.TipoDocumento).Where(c => c.IsActive == 1).OrderBy(c => c.Idproveedor).ToListAsync();
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
                var Only = await _dbContext.Proveedors.FirstOrDefaultAsync(c => c.Idproveedor.Equals(id));
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
                var Onlyproveedor = await _dbContext.Proveedors.FirstOrDefaultAsync(c => c.Idproveedor.Equals(id));
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

        private async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}

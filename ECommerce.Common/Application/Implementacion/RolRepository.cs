using AutoMapper;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.DataBase;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Common.Application.Implementacion
{
    public class RolRepository : GenericRepository<AspNetRole>, IRolRepository
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly IMapper _mapper;

        public RolRepository(ECommerceDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GenericResponse<RolDto>> DeactivateRolAsync(RolDto avatar)
        {
            try
            {
                var OnlyBodega = await _dbContext
                    .AspNetRoles.FirstOrDefaultAsync(c => c.RolId == avatar.RolId);
                OnlyBodega.IsActive = 0;
                _dbContext.AspNetRoles.Update(OnlyBodega);
                await SaveAllAsync();
                return new GenericResponse<RolDto> { IsSuccess = true, Result = avatar };

            }
            catch (Exception ex)
            {
                return new GenericResponse<RolDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<AspNetRole>> DeleteRolAsync(Guid id)
        {
            try
            {
                var bodega = await _dbContext.AspNetRoles.FirstOrDefaultAsync(c => c.RolId == id);
                if (bodega == null)
                {
                    return new GenericResponse<AspNetRole> { IsSuccess = false, Message = "No hay Datos!" };
                }

                bodega.IsActive = 0;

                _dbContext.AspNetRoles.Update(bodega);
                if (!await SaveAllAsync())
                {
                    return new GenericResponse<AspNetRole> { IsSuccess = false, Message = "La operacion no realizada!" };
                }

                return new GenericResponse<AspNetRole> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new GenericResponse<AspNetRole> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<List<RolDto>> GetAllRolesAsync()
        {
            var listAll = await _dbContext.AspNetRoles.Where(c => c.IsActive == 1).OrderBy(c => c.RolId).ToListAsync();
            var ListDto = new List<RolDto>();

            foreach (var list in listAll)
            {
                ListDto.Add(_mapper.Map<RolDto>(list));
            }
            return ListDto;
        }

        public async Task<GenericResponse<RolDto>> GetOnlyRolAsync(Guid id)
        {
            try
            {
                var Only = await _dbContext.AspNetRoles.FirstOrDefaultAsync(c => c.RolId.Equals(id));
                if (Only == null)
                {
                    return new GenericResponse<RolDto> { IsSuccess = false, Message = "No hay Datos!" };
                }
                var OnlyConcepto = _mapper.Map<RolDto>(Only);
                return new GenericResponse<RolDto> { IsSuccess = true, Result = OnlyConcepto };

            }
            catch (Exception ex)
            {
                return new GenericResponse<RolDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<AspNetRole>> OnlyRolGetAsync(Guid id)
        {
            try
            {
                var OnlyRol = await _dbContext.AspNetRoles.FirstOrDefaultAsync(c => c.RolId.Equals(id));
                if (OnlyRol == null)
                {
                    return new GenericResponse<AspNetRole> { IsSuccess = false, Message = "No hay Datos!" };
                }

                return new GenericResponse<AspNetRole> { IsSuccess = true, Result = OnlyRol };

            }
            catch (Exception ex)
            {
                return new GenericResponse<AspNetRole> { IsSuccess = false, Message = ex.Message };
            }
        }
        private async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}

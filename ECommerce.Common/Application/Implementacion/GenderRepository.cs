using AutoMapper;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.DataBase;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Common.Application.Implementacion
{
    public class GenderRepository : GenericRepository<Genero>, IGenderRepository
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenderRepository(ECommerceDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GenericResponse<GenderDto>> DeactivateGenderAsync(GenderDto avatar)
        {
            try
            {
                var OnlyDepart = await _dbContext
                    .Generos.FirstOrDefaultAsync(c => c.GenderId == avatar.GenderId);
                OnlyDepart.IsActive = 0;
                _dbContext.Generos.Update(OnlyDepart);
                await SaveAllAsync();
                return new GenericResponse<GenderDto> { IsSuccess = true, Result = avatar };

            }
            catch (Exception ex)
            {
                return new GenericResponse<GenderDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<Genero>> DeleteGenderAsync(int id)
        {
            try
            {
                var Depto = await _dbContext.Generos.FirstOrDefaultAsync(c => c.GenderId == id);
                if (Depto == null)
                {
                    return new GenericResponse<Genero> { IsSuccess = false, Message = "No hay Datos!" };
                }

                Depto.IsActive = 0;

                _dbContext.Generos.Update(Depto);
                if (!await SaveAllAsync())
                {
                    return new GenericResponse<Genero> { IsSuccess = false, Message = "La operacion no realizada!" };
                }

                return new GenericResponse<Genero> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new GenericResponse<Genero> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<List<GenderDto>> GetAllGenderAsync()
        {
            var listAll = await _dbContext.Generos.Where(c => c.IsActive == 1).OrderBy(c => c.GenderId).ToListAsync();
            var ListDto = new List<GenderDto>();

            foreach (var list in listAll)
            {
                ListDto.Add(_mapper.Map<GenderDto>(list));
            }
            return ListDto;
        }

        public async Task<GenericResponse<GenderDto>> GetOnlyGenderAsync(int id)
        {
            try
            {
                var Only = await _dbContext.Generos.FirstOrDefaultAsync(c => c.GenderId.Equals(id));
                if (Only == null)
                {
                    return new GenericResponse<GenderDto> { IsSuccess = false, Message = "No hay Datos!" };
                }
                var OnlyGneder = _mapper.Map<GenderDto>(Only);
                return new GenericResponse<GenderDto> { IsSuccess = true, Result = OnlyGneder };

            }
            catch (Exception ex)
            {
                return new GenericResponse<GenderDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<Genero>> OnlyGenderGetAsync(int id)
        {
            try
            {
                var OnlyGender = await _dbContext.Generos.FirstOrDefaultAsync(c => c.GenderId.Equals(id));
                if (OnlyGender == null)
                {
                    return new GenericResponse<Genero> { IsSuccess = false, Message = "No hay Datos!" };
                }

                return new GenericResponse<Genero> { IsSuccess = true, Result = OnlyGender };

            }
            catch (Exception ex)
            {
                return new GenericResponse<Genero> { IsSuccess = false, Message = ex.Message };
            }
        }

        private async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}

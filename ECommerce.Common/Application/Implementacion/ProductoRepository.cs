using AutoMapper;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.DataBase;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Common.Application.Implementacion
{
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        private readonly ECommerceDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductoRepository(ECommerceDbContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GenericResponse<ProductoDto>> DeactivateProductoAsync(ProductoDto avatar)
        {
            try
            {
                var OnlyProd = await _dbContext
                    .Productos.FirstOrDefaultAsync(c => c.Idproducto == avatar.Idproducto);
                OnlyProd.IsActive = 0;
                _dbContext.Productos.Update(OnlyProd);
                await SaveAllAsync();
                return new GenericResponse<ProductoDto> { IsSuccess = true, Result = avatar };

            }
            catch (Exception ex)
            {
                return new GenericResponse<ProductoDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<Producto>> DeleteProductoAsync(int id)
        {
            try
            {
                var producto = await _dbContext.Productos.FirstOrDefaultAsync(c => c.Idproducto == id);
                if (producto == null)
                {
                    return new GenericResponse<Producto> { IsSuccess = false, Message = "No hay Datos!" };
                }

                producto.IsActive = 0;

                _dbContext.Productos.Update(producto);
                if (!await SaveAllAsync())
                {
                    return new GenericResponse<Producto> { IsSuccess = false, Message = "La operacion no realizada!" };
                }

                return new GenericResponse<Producto> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new GenericResponse<Producto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<List<ProductoDto>> GetAllProductoAsync()
        {
            var listAll = await _dbContext.Productos
                .Include(d => d.Departamento)
                .Include(i => i.Iva)
                .Include(m => m.MedidaNavigation)
                .Where(c => c.IsActive == 1).ToListAsync();
            var ListDto = new List<ProductoDto>();

            foreach (var list in listAll)
            {
                ListDto.Add(_mapper.Map<ProductoDto>(list));
            }
            return ListDto;
        }

        public async Task<GenericResponse<ProductoDto>> GetOnlyProductoAsync(int id)
        {
            try
            {
                var Only = await _dbContext.Productos.FirstOrDefaultAsync(c => c.Idproducto.Equals(id));
                if (Only == null)
                {
                    return new GenericResponse<ProductoDto> { IsSuccess = false, Message = "No hay Datos!" };
                }
                var OnlyProd = _mapper.Map<ProductoDto>(Only);
                return new GenericResponse<ProductoDto> { IsSuccess = true, Result = OnlyProd };

            }
            catch (Exception ex)
            {
                return new GenericResponse<ProductoDto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<Producto>> OnlyProductoGetAsync(int id)
        {
            try
            {
                var Onlyprod = await _dbContext.Productos.FirstOrDefaultAsync(c => c.Idproducto.Equals(id));
                if (Onlyprod == null)
                {
                    return new GenericResponse<Producto> { IsSuccess = false, Message = "No hay Datos!" };
                }

                return new GenericResponse<Producto> { IsSuccess = true, Result = Onlyprod };

            }
            catch (Exception ex)
            {
                return new GenericResponse<Producto> { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<GenericResponse<ProductoDto>> ProductTransactionsAsync(ProductoDto avatar)
        {
            using (Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = _dbContext.Database.BeginTransaction()) {
                try
                {
                    transaction.Commit();
                    return new GenericResponse<ProductoDto>
                    {
                        IsSuccess = true,
                        Message = "Win - your data was changed successfully!",
                        Result = avatar
                    };
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    return new GenericResponse<ProductoDto>
                    {
                        IsSuccess = false,
                        Message = ex.Message,
                    };
                }
            }
        }

        private async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}

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
                    .Productos.FirstOrDefaultAsync(c => c.IdProducto == avatar.Idproducto);
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
                var producto = await _dbContext.Productos.FirstOrDefaultAsync(c => c.IdProducto == id);
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
                //.Include(d => d.Departamento)
                //.Include(i => i.Iva)
                //.Include(m => m.MedidaNavigation)
                //.Include(b => b.Barras)
                .Where(c => c.IsActive == 1).ToListAsync();
            var ListDto = new List<ProductoDto>();

            foreach (var list in listAll)
            {
                ListDto.Add(_mapper.Map<ProductoDto>(list));
            }
            return ListDto;
        }

        public async Task<List<Producto>> GetAllVMProductoAsync()
        {
            List<Producto> query = await _dbContext.Productos
                .Include(d => d.Departamento)
                .Include(m => m.MedidaNavigation)
                .Include(i => i.Iva)
                .Where(p=>p.IsActive==1).ToListAsync();
            return query;
        }

        public async Task<GenericResponse<ProductoDto>> GetOnlyProductoAsync(int id)
        {
            try
            {
                var Only = await _dbContext.Productos.FirstOrDefaultAsync(c => c.IdProducto.Equals(id));
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
                var Onlyprod = await _dbContext.Productos.FirstOrDefaultAsync(c => c.IdProducto.Equals(id));
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
                    Producto producto = new Producto() {
                        Descripcion = avatar.Descripcion,
                        Nombre = avatar.Nombre,
                        IsActive = 1,
                        DepartamentoId = avatar.DepartamentoId,
                        Ivaid = avatar.Ivaid,
                        MedidaId = avatar.MedidaId,
                        Medida = avatar.Medida,
                        Notas = avatar.Notas,
                        Precio = avatar.Precio,
                        Pieza = avatar.Pieza,
                        Imagen = avatar.Imagen,
                        PathImagen = avatar.PathImagen,
                        GuidImagen = avatar.GuidImagen, 
                    };
                    _dbContext.Productos.Add(producto);

                    await _dbContext.SaveChangesAsync();

                    Barra barCode = new Barra() { 
                        Idproducto = producto.IdProducto,
                        Barcode =  avatar.Barcode,
                    };

                    _dbContext.Barras.Add(barCode);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    return new GenericResponse<ProductoDto>
                    {
                        IsSuccess = true,
                        Message = "Win - your data was changed successfully!",
                        Result = avatar
                    };
                }
                catch (DbUpdateException dbUpdateException)
                {
                    transaction.Rollback();
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        return new GenericResponse<ProductoDto>
                        {
                            IsSuccess = false,
                            Message = "Ya existe una Producto  con el mismo nombre.",
                        };
                    }
                    else
                    {
                        return new GenericResponse<ProductoDto>
                        {
                            IsSuccess = false,
                            Message = dbUpdateException.InnerException.Message,
                        };
                    }
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

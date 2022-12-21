using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;

namespace ECommerce.Common.Application.Interfaces
{
    public interface IProductoRepository : IGenericRepositoryFactory<Producto>
    {
        Task<List<ProductoDto>> GetAllProductoAsync();
        Task<GenericResponse<ProductoDto>> GetOnlyProductoAsync(int id);
        Task<GenericResponse<Producto>> OnlyProductoGetAsync(int id);
        Task<GenericResponse<Producto>> DeleteProductoAsync(int id);
        Task<GenericResponse<ProductoDto>> DeactivateProductoAsync(ProductoDto avatar);
        Task<GenericResponse<ProductoDto>> ProductTransactionsAsync(ProductoDto avatar);
        Task<List<Producto>> GetAllVMProductoAsync();
    }
}

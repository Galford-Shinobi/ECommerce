using ECommerce.App.Helpers.Interfaces;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;
using static ECommerce.App.Helpers.ModalHelper;

namespace ECommerce.App.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IFlashMessage _flashMessagee;
        private readonly ICombosHelper _combosHelper;

        public ProductosController(IProductoRepository productoRepository, IFlashMessage flashMessagee, ICombosHelper combosHelper)
        {
            _productoRepository = productoRepository;
            _flashMessagee = flashMessagee;
            _combosHelper = combosHelper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _productoRepository.GetAllProductoAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                var model = new ProductoDto()
                {
                    ComboDepartamentos = _combosHelper.GetComboDepartamentos(),
                    ComboMedidas = _combosHelper.GetComboMedidums(),
                    ComboIvas = _combosHelper.GetComboIvas(),
                };
                return View(model);
            }
            else
            {
                var producto = await _productoRepository.GetOnlyProductoAsync(id);
                if (!producto.IsSuccess)
                {
                    return NotFound();
                }
                producto.Result.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                producto.Result.ComboMedidas = _combosHelper.GetComboMedidums();
                producto.Result.ComboIvas = _combosHelper.GetComboIvas();
                return View(producto.Result);
            }
        }
    }
}

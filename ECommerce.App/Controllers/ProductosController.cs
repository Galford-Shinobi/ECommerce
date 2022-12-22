using AutoMapper;
using ECommerce.App.Helpers;
using ECommerce.App.Helpers.Interfaces;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog.Fluent;
using Vereyon.Web;
using static ECommerce.App.Helpers.ModalHelper;

namespace ECommerce.App.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IFlashMessage _flashMessagee;
        private readonly ICombosHelper _combosHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductosController> _log;

        public ProductosController(IProductoRepository productoRepository, IFlashMessage flashMessagee, ICombosHelper combosHelper, IImageHelper imageHelper,IMapper mapper, ILogger<ProductosController> log)
        {
            _productoRepository = productoRepository;
            _flashMessagee = flashMessagee;
            _combosHelper = combosHelper;
            _imageHelper = imageHelper;
            _mapper = mapper;
            _log = log;
        }
        public async Task<IActionResult> Index()
        {
            List<VMBarraProducto> vmProductoLista = _mapper.Map<List<VMBarraProducto>>(await _productoRepository.GetAllVMBarraProductoAsync());
            return View(vmProductoLista);
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
                    _log.LogError($"ERROR: {producto.Message}");
                    return NotFound();
                }

                var code = await _productoRepository.RetrieveBarcode(id);
                if (!code.IsSuccess)
                {
                    _log.LogError($"ERROR: {code.Message}");
                    return NotFound();
                }
                producto.Result.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                producto.Result.ComboMedidas = _combosHelper.GetComboMedidums();
                producto.Result.ComboIvas = _combosHelper.GetComboIvas();
                producto.Result.Barcode = code.Result.Barcode;
                return View(producto.Result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, ProductoDto avatar)
        {
            if (ModelState.IsValid)
            {
                Producto OnlyProd = new Producto();
                try
                {
                    string path = string.Empty;
                    byte[] pathByteArray = new byte[0];
                    string pathGuid = string.Empty;

                    if (id == 0) //Insert
                    {

                        if (avatar.ImageFile != null)
                        {
                            path = await _imageHelper.UploadImageAsync(avatar.ImageFile, "Products");
                            pathByteArray = await _imageHelper.UploadImageArrayAsync(avatar.ImageFile);//model.ImageId = path;
                        }

                        avatar.Imagen = pathByteArray;
                        avatar.PathImagen = path;
                        avatar.GuidImagen  = Guid.NewGuid();

                        var ResultOnly = await _productoRepository.ProductTransactionsAsync(avatar);
                        if (ResultOnly.IsSuccess)
                        {
                            _flashMessagee.Info("Registro creado.");
                        }
                        else { _flashMessagee.Danger(ResultOnly.Message); _log.LogError($"ERROR: {ResultOnly.Message}"); }
                    }
                    else //Update
                    {

                        if (id != avatar.Idproducto)
                        {
                            _flashMessagee.Danger("Los datos son incorrectos!");
                            _log.LogError($"ERROR: Los datos son incorrectos!");
                            avatar.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                            avatar.ComboMedidas = _combosHelper.GetComboMedidums();
                            avatar.ComboIvas = _combosHelper.GetComboIvas();
                            return View(avatar);
                        }
                         pathByteArray = avatar.Imagen;
                         path = avatar.PathImagen;
                         
                        if (avatar.ImageFile != null)
                        {
                            path = await _imageHelper.UploadImageAsync(avatar.ImageFile, "Products");
                            pathByteArray = await _imageHelper.UploadImageArrayAsync(avatar.ImageFile);//model.ImageId = path;
                        }

                        avatar.Imagen = pathByteArray;
                        avatar.PathImagen = path;
                        avatar.GuidImagen = avatar.GuidImagen;

                        var Only = await _productoRepository.ProductTransactionsUpdateAsync(avatar);

                        if (!Only.IsSuccess)
                        {
                            _log.LogError($"ERROR: {Only.Message}");
                          
                            _flashMessagee.Danger(Only.Message); ;
                        }
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _log.LogError("ERROR: " + "Ya existe una categoría con el mismo nombre.");
                        _flashMessagee.Danger("Ya existe una categoría con el mismo nombre.");
                    }
                    else
                    {
                        _log.LogError($"ERROR: {dbUpdateException.InnerException.Message}");
                        _flashMessagee.Danger(dbUpdateException.InnerException.Message);
                    }
                    avatar.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                    avatar.ComboMedidas = _combosHelper.GetComboMedidums();
                    avatar.ComboIvas = _combosHelper.GetComboIvas();
                    return View(avatar);
                }
                catch (Exception exception)
                {
                    _log.LogError("ERROR: " + exception);
                    _flashMessagee.Danger(exception.Message);
                    avatar.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                    avatar.ComboMedidas = _combosHelper.GetComboMedidums();
                    avatar.ComboIvas = _combosHelper.GetComboIvas();
                    return View(avatar);
                }
                List<VMBarraProducto> vmProductoLista = _mapper.Map<List<VMBarraProducto>>(await _productoRepository.GetAllVMBarraProductoAsync());
                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", vmProductoLista) });

            }

            avatar.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
            avatar.ComboMedidas = _combosHelper.GetComboMedidums();
            avatar.ComboIvas = _combosHelper.GetComboIvas();
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", avatar) });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prod = await _productoRepository.GetOnlyProductoAsync(id.Value);
            if (!prod.IsSuccess)
            {
                _log.LogError($"ERROR: {prod.Message}");
                return NotFound();
            }

            var onlyProd = await _productoRepository.DeactivateProductoAsync(prod.Result);

            if (!onlyProd.IsSuccess)
            {
                _log.LogError($"ERROR: {onlyProd.Message}");
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

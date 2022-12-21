using AutoMapper;
using ECommerce.App.Helpers;
using ECommerce.App.Helpers.Interfaces;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public ProductosController(IProductoRepository productoRepository, IFlashMessage flashMessagee, ICombosHelper combosHelper, IImageHelper imageHelper,IMapper mapper)
        {
            _productoRepository = productoRepository;
            _flashMessagee = flashMessagee;
            _combosHelper = combosHelper;
            _imageHelper = imageHelper;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<VMProducto> vmProductoLista = _mapper.Map<List<VMProducto>>(await _productoRepository.GetAllVMProductoAsync());
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
                    return NotFound();
                }
                producto.Result.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                producto.Result.ComboMedidas = _combosHelper.GetComboMedidums();
                producto.Result.ComboIvas = _combosHelper.GetComboIvas();
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
                        else { _flashMessagee.Danger(ResultOnly.Message); }
                    }
                    else //Update
                    {

                        if (id != avatar.Idproducto)
                        {
                            _flashMessagee.Danger("Los datos son incorrectos!");
                            avatar.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                            avatar.ComboMedidas = _combosHelper.GetComboMedidums();
                            avatar.ComboIvas = _combosHelper.GetComboIvas();
                            return View(avatar);
                        }
                         pathByteArray = avatar.Imagen;
                         path = avatar.PathImagen;
                         pathGuid = avatar.GuidImagen.ToString();
                        if (avatar.ImageFile != null)
                        {
                            path = await _imageHelper.UploadImageAsync(avatar.ImageFile, "Products");
                            pathByteArray = await _imageHelper.UploadImageArrayAsync(avatar.ImageFile);//model.ImageId = path;
                        }
                        //var Only = await _medidumRepository.OnlyMedidumGetAsync(avatar.MedidaId);

                        //if (!Only.IsSuccess)
                        //{
                        //    return NotFound();
                        //}
                        //OnlyMedid = Only.Result;
                        //OnlyMedid.Descripcion = (avatar.Descripcion == Only.Result.Descripcion) ? Only.Result.Descripcion : avatar.Descripcion;
                        //OnlyMedid.Escala = (avatar.Escala == Only.Result.Escala) ? Only.Result.Escala : avatar.Escala;
                        //var result = await _medidumRepository.UpdateDataAsync(OnlyMedid);

                        //if (result.IsSuccess)
                        //    _flashMessagee.Info("Registro actualizado.");
                        //else _flashMessagee.Danger(result.Message);
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessagee.Danger("Ya existe una categoría con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessagee.Danger(dbUpdateException.InnerException.Message);
                    }
                    avatar.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                    avatar.ComboMedidas = _combosHelper.GetComboMedidums();
                    avatar.ComboIvas = _combosHelper.GetComboIvas();
                    return View(avatar);
                }
                catch (Exception exception)
                {
                    _flashMessagee.Danger(exception.Message);
                    avatar.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
                    avatar.ComboMedidas = _combosHelper.GetComboMedidums();
                    avatar.ComboIvas = _combosHelper.GetComboIvas();
                    return View(avatar);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", await _productoRepository.GetAllProductoAsync()) });

            }

            avatar.ComboDepartamentos = _combosHelper.GetComboDepartamentos();
            avatar.ComboMedidas = _combosHelper.GetComboMedidums();
            avatar.ComboIvas = _combosHelper.GetComboIvas();
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", avatar) });
        }
    }
}

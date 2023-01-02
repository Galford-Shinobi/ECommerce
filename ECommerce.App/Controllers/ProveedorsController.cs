using AutoMapper;
using ECommerce.App.Helpers;
using ECommerce.App.Helpers.Interfaces;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;
using static ECommerce.App.Helpers.ModalHelper;

namespace ECommerce.App.Controllers
{
    public class ProveedorsController : Controller
    {
        private readonly IProveedorRepository _proveedorRepository;
        private readonly IFlashMessage _flashMessagee;
        private readonly ICombosHelper _combosHelper;
        private readonly IMapper _mapper;
        private readonly ILogger<ProveedorsController> _log;

        public ProveedorsController(IProveedorRepository proveedorRepository, IFlashMessage flashMessagee,
            ICombosHelper combosHelper, IMapper mapper, ILogger<ProveedorsController> log)
        {
            _proveedorRepository = proveedorRepository;
            _flashMessagee = flashMessagee;
            _combosHelper = combosHelper;
            _mapper = mapper;
            _log = log;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _proveedorRepository.GetAllProveedorAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            ProveedorViewModel model = new ProveedorViewModel();
            if (id == 0)
            {
                model.ComboTipoDocumentos = _combosHelper.GetComboTipoDocuemtnos();

                return View(model);
            }
            else
            {
                var Provee = await _proveedorRepository.OnlyProveedorGetAsync(id);
                if (!Provee.IsSuccess)
                {
                    _log.LogError($"ERROR: {Provee.ErrorMessage}{" "}{Provee.Message}");
                    return NotFound();
                }
                model.Documento = Provee.Result.Documento;
                model.TipoDocumentoId = Provee.Result.TipoDocumentoId;
                model.Documento = Provee.Result.Documento;
                model.Idproveedor = Provee.Result.Idproveedor;
                model.Telefono1 = Provee.Result.Telefono1;
                model.Telefono2 = Provee.Result.Telefono2;
                model.ApellidosContacto = Provee.Result.ApellidosContacto;
                model.ComboTipoDocumentos = _combosHelper.GetComboTipoDocuemtnos();
                model.Nombre = Provee.Result.Nombre;
                model.Correo = Provee.Result.Correo;
                model.Direccion = Provee.Result.Direccion;
                model.NombresContacto = Provee.Result.NombresContacto;
                model.Notas = Provee.Result.Notas;
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, ProveedorViewModel avatar) {
            if (ModelState.IsValid) {
                Proveedor model = new Proveedor();
                try
                {
                    if (id == 0) //Insert
                    {
                        model.Nombre = avatar.Nombre;
                        model.Correo = avatar.Correo;
                        model.NombresContacto = avatar.NombresContacto;
                        model.Notas = avatar.Notas;
                        model.TipoDocumentoId = avatar.TipoDocumentoId;
                        model.ApellidosContacto = avatar.ApellidosContacto;
                        model.Direccion=avatar.Direccion;
                        model.Documento = avatar.Documento;
                        model.IsActive = 1;
                        model.RegistrationDate= DateTime.Now.ToUniversalTime();
                        model.Telefono1 = avatar.Telefono1;
                        model.Telefono2 = avatar.Telefono2;
                        var ResultOnly = await _proveedorRepository.AddDataAsync(model);
                        if (ResultOnly.IsSuccess)
                        {
                            _flashMessagee.Info("Registro creado.");
                            _log.LogInformation("Registro creado.");
                        }
                        else { _flashMessagee.Danger(ResultOnly.Message); _log.LogError($"Repository - Error : {ResultOnly.ErrorMessage}{" "}{ResultOnly.Message}"); }
                    }
                    else {
                        if (id != avatar.Idproveedor)
                        {
                            _flashMessagee.Danger("Los datos son incorrectos!");
                            _log.LogError($"Id Proveedor - ERROR: Los datos son incorrectos!");
                            avatar.ComboTipoDocumentos= _combosHelper.GetComboTipoDocuemtnos();
                            
                            return View(avatar);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _log.LogError($"Exception : {ex.InnerException.Message}{" "}");
                }
                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", await _proveedorRepository.GetAllProveedorAsync()) });
            }
            avatar.ComboTipoDocumentos = _combosHelper.GetComboTipoDocuemtnos();
            
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", avatar) });
        }
    }
}

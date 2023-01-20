using ECommerce.App.Helpers;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Vereyon.Web;
using static ECommerce.App.Helpers.ModalHelper;

namespace ECommerce.App.Controllers
{
    [Authorize(Roles = UserRolesResponsive.AdminSuperUser)]
    public class BodegasController : Controller
    {
        private readonly IBodegaRepository _bodegaRepository;
        private readonly IFlashMessage _flashMessagee;
        private readonly ILogger<BodegasController> _log;

        public BodegasController(IBodegaRepository bodegaRepository, IFlashMessage flashMessagee, ILogger<BodegasController> log)
        {
            _bodegaRepository = bodegaRepository;
            _flashMessagee = flashMessagee;
            _log = log;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _bodegaRepository.GetAllBodegaAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new BodegaDto());
            }
            else
            {
                var bodega = await _bodegaRepository.GetOnlyBodegaAsync(id);
                if (!bodega.IsSuccess)
                {
                    _log.LogError($"ERROR: {bodega.Message}");
                    return NotFound();
                }

                return View(bodega.Result);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, BodegaDto avatar)
        {
            if (ModelState.IsValid)
            {
                Bodega OnlyBodega = new Bodega();
                try
                {
                    if (id == 0) //Insert
                    {


                        OnlyBodega.Descripcion = avatar.Descripcion;
                        OnlyBodega.RegistrationDate = DateTime.Now.ToUniversalTime();
                        OnlyBodega.IsActive = 1;
                        var ResultOnly = await _bodegaRepository.AddDataAsync(OnlyBodega);
                        if (ResultOnly.IsSuccess)
                        {
                            _flashMessagee.Info("Registro creado.");
                        }
                        else { _flashMessagee.Danger(ResultOnly.Message); _log.LogError($"ERROR: {ResultOnly.ErrorMessage}{" "}{ResultOnly.Message}"); }
                    }
                    else //Update
                    {

                        if (id != avatar.BodegaId)
                        {
                            _flashMessagee.Danger("Los datos son incorrectos!");
                            _log.LogError($"ERROR: {"Los datos son incorrectos!"}");
                            return View(avatar);
                        }

                        var Only = await _bodegaRepository.OnlyBodegaGetAsync(avatar.BodegaId);

                        if (!Only.IsSuccess)
                        {
                            _log.LogError($"ERROR: {Only.ErrorMessage}{" "}{Only.Message}");
                            return NotFound();
                        }
                        OnlyBodega = Only.Result;
                        OnlyBodega.Descripcion = (avatar.Descripcion == Only.Result.Descripcion) ? Only.Result.Descripcion : avatar.Descripcion;
                        var result = await _bodegaRepository.UpdateDataAsync(OnlyBodega);

                        if (result.IsSuccess)
                            _flashMessagee.Info("Registro actualizado.");
                        else _flashMessagee.Danger(result.Message);
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _log.LogError($"ERROR: {"Ya existe una bodega con el mismo nombre "}");
                        _flashMessagee.Danger("Ya existe una bodega con el mismo nombre.");
                    }
                    else
                    {
                        _log.LogError($"ERROR: {dbUpdateException.InnerException.Message}");
                        _flashMessagee.Danger(dbUpdateException.InnerException.Message);
                    }
                    return View(avatar);
                }
                catch (Exception exception)
                {
                    _log.LogError($"ERROR: {exception}");
                    _flashMessagee.Danger(exception.Message);
                    return View(avatar);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", await _bodegaRepository.GetAllBodegaAsync()) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", avatar) });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _log.LogError($"ERROR: {"informacion falta del ID"}");
                return NotFound();
            }

            var bodega = await _bodegaRepository.GetOnlyBodegaAsync(id.Value);
            if (!bodega.IsSuccess)
            {
                _log.LogError($"ERROR: {bodega.Message}{" "} {bodega.ErrorMessage}");
                return NotFound();
            }

            var onlyConcep = await _bodegaRepository.DeactivateBodegaAsync(bodega.Result);

            if (!onlyConcep.IsSuccess)
            {
                _log.LogError($"ERROR: {onlyConcep.Message}{" "} {onlyConcep.ErrorMessage}");
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

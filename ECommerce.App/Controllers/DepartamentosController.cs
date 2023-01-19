using ECommerce.App.Helpers;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Vereyon.Web;
using static ECommerce.App.Helpers.ModalHelper;

namespace ECommerce.App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DepartamentosController : Controller
    {
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IFlashMessage _flashMessagee;
        private readonly ILogger<DepartamentosController> _log;

        public DepartamentosController(IDepartamentoRepository departamentoRepository, IFlashMessage flashMessagee, ILogger<DepartamentosController> log)
        {
            _departamentoRepository = departamentoRepository;
            _flashMessagee = flashMessagee;
            _log = log;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _departamentoRepository.GetAllDepartamentoAsync());
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new DepartamentoDto());
            }
            else
            {
                var Depart = await _departamentoRepository.GetOnlyDepartamentoAsync(id);
                if (!Depart.IsSuccess)
                {
                    _log.LogError($"ERROR: {Depart.ErrorMessage}{" "}{Depart.Message}");
                    return NotFound();
                }

                return View(Depart.Result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, DepartamentoDto avatar)
        {
            if (ModelState.IsValid)
            {
                Departamento OnlyDept = new Departamento();
                try
                {
                    if (id == 0) //Insert
                    {


                        OnlyDept.Descripcion = avatar.Descripcion;
                        OnlyDept.RegistrationDate = DateTime.Now.ToUniversalTime();
                        OnlyDept.IsActive = 1;
                        var ResultOnly = await _departamentoRepository.AddDataAsync(OnlyDept);
                        if (ResultOnly.IsSuccess)
                        {
                            _log.LogError($"ERROR : {ResultOnly.ErrorMessage}{" "}{ResultOnly.Message}");
                            _flashMessagee.Info("Registro creado.");
                        }
                        else { _flashMessagee.Danger(ResultOnly.Message); }
                    }
                    else //Update
                    {

                        if (id != avatar.DepartamentoId)
                        {
                            _flashMessagee.Danger("Los datos son incorrectos!");
                            _log.LogError($"ERROR: los datos son incorrectos!");
                            return View(avatar);
                        }

                        var Only = await _departamentoRepository.OnlyDepartamentoGetAsync(avatar.DepartamentoId);

                        if (!Only.IsSuccess)
                        {
                            _log.LogError($"ERROR: {Only.ErrorMessage}{" "}{Only.Message}");
                            return NotFound();
                        }
                        OnlyDept = Only.Result;
                        OnlyDept.Descripcion = (avatar.Descripcion == Only.Result.Descripcion) ? Only.Result.Descripcion : avatar.Descripcion;
                        var result = await _departamentoRepository.UpdateDataAsync(OnlyDept);

                        if (result.IsSuccess)
                            _flashMessagee.Info("Registro actualizado.");
                        else _flashMessagee.Danger(result.Message); _log.LogError($"ERROR: {result.ErrorMessage}{" "}{result.Message}"); ;
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessagee.Danger("Ya existe una categoría con el mismo nombre.");
                        _log.LogError($"ERROR: {"Ya existe un departamento con el mismo nombre"}");
                    }
                    else
                    {
                        _flashMessagee.Danger(dbUpdateException.InnerException.Message);
                        _log.LogError($"ERROR: {dbUpdateException.InnerException.Message}");
                    }
                    return View(avatar);
                }
                catch (Exception exception)
                {
                    _flashMessagee.Danger(exception.Message);
                    _log.LogError($"ERROR: {exception}");
                    return View(avatar);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", await _departamentoRepository.GetAllDepartamentoAsync()) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", avatar) });
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _log.LogError($"ERROR: {"datos incorrectos!"}");
                return NotFound();
            }

            var depto = await _departamentoRepository.GetOnlyDepartamentoAsync(id.Value);
            if (!depto.IsSuccess)
            {
                _log.LogError($"ERROR: {depto.ErrorMessage}{" "}{depto.Message}");
                return NotFound();
            }

            var onlyDepto = await _departamentoRepository.DeactivateDepartamentoAsync(depto.Result);

            if (!onlyDepto.IsSuccess)
            {
                _log.LogError($"ERROR: {onlyDepto.ErrorMessage}{" "}{onlyDepto.Message}");
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

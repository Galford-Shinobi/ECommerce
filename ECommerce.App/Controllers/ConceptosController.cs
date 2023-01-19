using AutoMapper;
using ECommerce.App.Helpers;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using Vereyon.Web;
using static ECommerce.App.Helpers.ModalHelper;

namespace ECommerce.App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ConceptosController : Controller
    {
        private readonly IConceptoRepository _conceptoRepository;
        private readonly IFlashMessage _flashMessagee;
        private readonly ILogger<ConceptosController> _log;

        public ConceptosController(IConceptoRepository conceptoRepository, IFlashMessage flashMessagee, ILogger<ConceptosController> log)
        {
            _conceptoRepository = conceptoRepository;
            _flashMessagee = flashMessagee;
            _log = log;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _conceptoRepository.GetAllConceptoAsync());
        }
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new ConceptoDto());
            }
            else
            {
                var concepto = await _conceptoRepository.GetOnlyConceptoAsync(id);
                if (!concepto.IsSuccess)
                {
                    _log.LogError($"ERROR: {concepto.ErrorMessage}{" "}{concepto.Message}");
                    return NotFound();
                }

                return View(concepto.Result);
            }
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, ConceptoDto avatar)
        {
            if (ModelState.IsValid)
            {
                Concepto OnlyConcepto = new Concepto();
                try
                {
                    if (id == 0) //Insert
                    {


                        OnlyConcepto.Descripcion = avatar.Descripcion;
                        OnlyConcepto.RegistrationDate = DateTime.Now.ToUniversalTime();
                        OnlyConcepto.IsActive = 1;
                     var ResultOnly = await _conceptoRepository.AddDataAsync(OnlyConcepto);
                        if (ResultOnly.IsSuccess)
                        {
                            _flashMessagee.Info("Registro creado.");
                        }
                        else { _flashMessagee.Danger(ResultOnly.Message); }
                    }
                    else //Update
                    {

                        if (id != avatar.ConceptoId)
                        {
                            _flashMessagee.Danger("Los datos son incorrectos!");
                            return View(avatar);
                        }

                       var  Only = await _conceptoRepository.OnlyConceptoGetAsync(avatar.ConceptoId);

                        if (!Only.IsSuccess)
                        {
                            return NotFound();
                        }
                        OnlyConcepto = Only.Result;
                        OnlyConcepto.Descripcion = (avatar.Descripcion == Only.Result.Descripcion)? Only.Result.Descripcion : avatar.Descripcion;
                     var result =  await _conceptoRepository.UpdateDataAsync(OnlyConcepto);

                        if (result.IsSuccess)
                        _flashMessagee.Info("Registro actualizado.");
                        else _flashMessagee.Danger(result.Message);
                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _log.LogError($"ERROR: {"Ya existe una concepto con el mismo nombre."}");
                        _flashMessagee.Danger("Ya existe una categoría con el mismo nombre.");
                    }
                    else
                    {
                        _log.LogError($"ERROR: {dbUpdateException.Message}{" "}{dbUpdateException}");
                        _flashMessagee.Danger(dbUpdateException.InnerException.Message);
                    }
                    return View(avatar);
                }
                catch (Exception exception)
                {
                    _flashMessagee.Danger(exception.Message);
                    _log.LogError($"ERROR: {exception}");
                    return View(avatar);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", await _conceptoRepository.GetAllConceptoAsync()) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", avatar) });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _log.LogError($"ERROR: {"informacion incorrecta!"}");
                return NotFound();
            }

            var  concepto = await _conceptoRepository.GetOnlyConceptoAsync(id.Value);
            if (!concepto.IsSuccess)
            {
                _log.LogError($"ERROR: {concepto.ErrorMessage}{" "}{concepto.Message}");
                return NotFound();
            }

           var onlyConcep = await _conceptoRepository.DeactivateConceptoAsync(concepto.Result);

            if (!onlyConcep.IsSuccess)
            {
                _log.LogError($"ERROR: {onlyConcep.ErrorMessage}{" "}{onlyConcep.Message}");
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

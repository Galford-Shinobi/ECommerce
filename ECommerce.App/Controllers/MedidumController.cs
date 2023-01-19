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
    public class MedidumController : Controller
    {
        private readonly IMedidumRepository _medidumRepository;
        private readonly IFlashMessage _flashMessagee;

        public MedidumController(IMedidumRepository medidumRepository, IFlashMessage flashMessagee)
        {
            _medidumRepository = medidumRepository;
            _flashMessagee = flashMessagee;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _medidumRepository.GetAllMedidumAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<MedidumDto> vmmedidumLista = await _medidumRepository.GetAllMedidumAsync();

            return StatusCode(StatusCodes.Status200OK, new { data = vmmedidumLista });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new MedidumDto());
            }
            else
            {
                var concepto = await _medidumRepository.GetOnlyMedidumAsync(id);
                if (!concepto.IsSuccess)
                {
                    return NotFound();
                }

                return View(concepto.Result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, MedidumDto avatar)
        {
            if (ModelState.IsValid)
            {
                Medidum OnlyMedid = new Medidum();
                try
                {
                    if (id == 0) //Insert
                    {


                        OnlyMedid.Descripcion = avatar.Descripcion;
                        OnlyMedid.Escala = avatar.Escala;
                        OnlyMedid.RegistrationDate = DateTime.Now.ToUniversalTime();
                        OnlyMedid.IsActive = 1;
                        var ResultOnly = await _medidumRepository.AddDataAsync(OnlyMedid);
                        if (ResultOnly.IsSuccess)
                        {
                            _flashMessagee.Info("Registro creado.");
                        }
                        else { _flashMessagee.Danger(ResultOnly.Message); }
                    }
                    else //Update
                    {

                        if (id != avatar.MedidaId)
                        {
                            _flashMessagee.Danger("Los datos son incorrectos!");
                            return View(avatar);
                        }

                        var Only = await _medidumRepository.OnlyMedidumGetAsync(avatar.MedidaId);

                        if (!Only.IsSuccess)
                        {
                            return NotFound();
                        }
                        OnlyMedid = Only.Result;
                        OnlyMedid.Descripcion = (avatar.Descripcion == Only.Result.Descripcion) ? Only.Result.Descripcion : avatar.Descripcion;
                        OnlyMedid.Escala = (avatar.Escala == Only.Result.Escala) ? Only.Result.Escala : avatar.Escala;
                        var result = await _medidumRepository.UpdateDataAsync(OnlyMedid);

                        if (result.IsSuccess)
                            _flashMessagee.Info("Registro actualizado.");
                        else _flashMessagee.Danger(result.Message);
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
                    return View(avatar);
                }
                catch (Exception exception)
                {
                    _flashMessagee.Danger(exception.Message);
                    return View(avatar);
                }

                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", await _medidumRepository.GetAllMedidumAsync()) });

            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", avatar) });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medidum = await _medidumRepository.GetOnlyMedidumAsync(id.Value);
            if (!medidum.IsSuccess)
            {
                return NotFound();
            }

            var onlyMedidum = await _medidumRepository.DeactivateMedidumAsync(medidum.Result);

            if (!onlyMedidum.IsSuccess)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}

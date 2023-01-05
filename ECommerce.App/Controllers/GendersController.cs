using ECommerce.App.Helpers;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static ECommerce.App.Helpers.ModalHelper;

namespace ECommerce.App.Controllers
{
    public class GendersController : Controller
    {
        private readonly IGenderRepository _genderRepository;
        private readonly ILogger<GendersController> _log;
        private readonly IFlashMessage _flashMessagee;

        public GendersController(IGenderRepository genderRepository, ILogger<GendersController> log, IFlashMessage flashMessagee)
        {
            _genderRepository = genderRepository;
            _log = log;
            _flashMessagee = flashMessagee;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _genderRepository.GetAllGenderAsync());
        }
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new GenderDto());
            }
            else
            {
                var gender = await _genderRepository.GetOnlyGenderAsync(id);
                if (!gender.IsSuccess)
                {
                    _log.LogError($"ERROR: {gender.ErrorMessage}{" "}{gender.Message}");
                    return NotFound();
                }

                return View(gender.Result);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, GenderDto avatar) {
            if (ModelState.IsValid){
                try
                {
                    Genero OnlyGenero = new Genero();
                    if (id == 0) //Insert
                    {
                        OnlyGenero.GeneroName = avatar.GeneroName;
                        OnlyGenero.Description = avatar.Description;
                        OnlyGenero.RegistrationDate = DateTime.Now.ToUniversalTime();
                        OnlyGenero.IsActive = 1;
                        var ResultOnly = await _genderRepository.AddDataAsync(OnlyGenero);
                        if (ResultOnly.IsSuccess)
                        {
                            _flashMessagee.Info("Registro creado.");
                        }
                        else { _flashMessagee.Danger(ResultOnly.Message); _log.LogError($"ERROR: {ResultOnly.ErrorMessage}{" "}{ResultOnly.Message}"); }
                    }
                    else //Update
                    {
                        if (id != avatar.GenderId)
                        {
                            _flashMessagee.Danger("Los datos son incorrectos!");
                            _log.LogError($"ERROR: {"Los datos son incorrectos!"}");
                            return View(avatar);
                        }
                        var Only = await _genderRepository.OnlyGenderGetAsync(avatar.GenderId);

                        if (!Only.IsSuccess)
                        {
                            _log.LogError($"ERROR: {Only.ErrorMessage}{" "}{Only.Message}");
                            return NotFound();
                        }
                        OnlyGenero = Only.Result;
                        OnlyGenero.Description = (avatar.Description == Only.Result.Description) ? Only.Result.Description : avatar.Description;
                        OnlyGenero.GeneroName = (avatar.GeneroName == Only.Result.GeneroName) ? Only.Result.GeneroName : avatar.GeneroName;
                        var result = await _genderRepository.UpdateDataAsync(OnlyGenero);

                    }
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessagee.Danger("Ya existe una Genero con el mismo nombre.");
                        _log.LogError($"ERROR: {"Ya existe un Genero con el mismo nombre"}");
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
                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", await _genderRepository.GetAllGenderAsync()) });
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

            var genero = await _genderRepository.GetOnlyGenderAsync(id.Value);
            if (!genero.IsSuccess)
            {
                _log.LogError($"ERROR: {genero.Message}{" "} {genero.ErrorMessage}");
                return NotFound();
            }

            var onlyGenero = await _genderRepository.DeactivateGenderAsync(genero.Result);

            if (!onlyGenero.IsSuccess)
            {
                _log.LogError($"ERROR: {onlyGenero.Message}{" "} {onlyGenero.ErrorMessage}");
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

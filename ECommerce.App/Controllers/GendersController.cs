using ECommerce.App.Helpers;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Application.Interfaces;
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
                    if (id == 0) //Insert
                    { }
                    else { }
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
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", avatar) });
        }
    }
}

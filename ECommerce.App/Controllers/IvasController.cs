using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Vereyon.Web;

namespace ECommerce.App.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class IvasController : Controller
    {
        private readonly IIvaRepository _ivaRepository;
        private readonly IFlashMessage _flashMessagee;

        public IvasController(IIvaRepository ivaRepository, IFlashMessage flashMessagee)
        {
            _ivaRepository = ivaRepository;
            _flashMessagee = flashMessagee;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> listaIvas()
        {
            List<IvaDto> _lista = await _ivaRepository.GetAllIvaAsync();

            return StatusCode(StatusCodes.Status200OK, _lista);
        }

        [HttpPost]
        public async Task<IActionResult> guardarIvas([FromBody] Iva modelo)
        {
            var  _resultado = await _ivaRepository.AddDataAsync(modelo);

            if (_resultado.IsSuccess)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado.Result, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado.Result, msg = _resultado.Message });
        }

        [HttpPut]
        public async Task<IActionResult> editarIvas([FromBody] Iva modelo)
        {
            var _resultado = await _ivaRepository.UpdateDataAsync(modelo);

            if (_resultado.IsSuccess)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado.Result, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado.Result, msg = _resultado.Message });
        }

        [HttpDelete]
        public async Task<IActionResult> eliminarIvas(int ivaId)
        {
            var _resultado = await _ivaRepository.DeleteIvaAsync(ivaId);

            if (_resultado.IsSuccess)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado.IsSuccess, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado.IsSuccess, msg = _resultado.Message });
        }

        [HttpGet]
        public async Task<IActionResult> Lista()
        {
            List<IvaDto> vmIvaLista = await _ivaRepository.GetAllIvaAsync();

            return StatusCode(StatusCodes.Status200OK, new { data = vmIvaLista });
        }
    }
}

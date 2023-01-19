using ECommerce.App.Helpers;
using ECommerce.App.Helpers.Interfaces;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Models;
using ECommerce.Common.Models.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Security.Claims;
using Vereyon.Web;

namespace ECommerce.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserFactoryRepository _userFactoryRepository;
        private readonly ICombosHelper _combosHelper;
        private readonly IFlashMessage _flashMessage;
        private readonly ILogger<BodegasController> _log;

        public AccountController(IConfiguration configuration, IUserFactoryRepository userFactoryRepository
            , ICombosHelper combosHelper, IFlashMessage flashMessagee, ILogger<BodegasController> log)
        {
            _configuration = configuration;
            _userFactoryRepository = userFactoryRepository;
           _combosHelper = combosHelper;
           _flashMessage = flashMessagee;
           _log = log;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        [HttpGet]
        public IActionResult MyLoginPartial()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyLoginPartial(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var LoginTokenUser = await _userFactoryRepository.LoginAsync(model);

                    if (!LoginTokenUser.IsSuccess)
                    {
                        switch (LoginTokenUser.TruePasswordHash)
                        {
                            case -1:
                                // code block
                                ModelState.AddModelError(string.Empty, $"{LoginTokenUser.Message}");
                                break;
                            case -03:
                                // code block
                                ModelState.AddModelError(string.Empty, $"{LoginTokenUser.Message}");
                                break;
                            case -5:
                                // code block
                                ModelState.AddModelError(string.Empty, $"{LoginTokenUser.Message}");
                                break;
                            case -10:
                                // code block
                                ModelState.AddModelError(string.Empty, $"{LoginTokenUser.Message}");
                                return RedirectToAction("ConfirmPassword", "Account");
                            case -20:
                                ModelState.AddModelError(string.Empty, $"{LoginTokenUser.Message}");
                                break;
                            case -30:
                                ModelState.AddModelError(string.Empty, $"{LoginTokenUser.Message}");
                                break;
                        }
                        return View(model);
                    }
                    var RolUser = (UserResponseDto)LoginTokenUser.Result;
                    var claims = new List<Claim>
                    {
                        new Claim("user", RolUser.UserName),
                        new Claim("Role", RolUser.RolName),
                        new Claim("UserId", RolUser.UserId.ToString()),
                    };

                    //foreach (string rol in usuario.Roles)
                    //{
                    //    claims.Add(new Claim(ClaimTypes.Role, rol));
                    //}

                    await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "user", "Role")));

                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }


                    return RedirectToAction("Index", "Home");
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError(string.Empty, $"{exc.Message}");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        //[Authorize(Roles = "Adminstrador,Distribuidor,MesaControl")]
        //[Authorize(Roles = UserRoles.AdminFull)]
        public async Task<IActionResult> Register() 
        {
            if (User.Identity.Name == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }
            var _users = await _userFactoryRepository.GetUserByEmailAsync(User.Identity.Name);

            if (!_users.IsSuccess || _users.Result == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var model = new AddUserViewModel
            {
                Password = _configuration["SecretP:SecretPassword"],
                PasswordConfirm = _configuration["SecretP:SecretPassword"],
                ComboGeneros = _combosHelper.GetComboGeneros(),
                Dni = "A-0000000"
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeUser()
        {
            if (User.Identity.Name == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }

            var _users = await _userFactoryRepository.GetUserByEmailAsync(User.Identity.Name);

            if (!_users.IsSuccess || _users.Result == null)
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }



            EditUserViewModel model = new EditUserViewModel
            {
                FirstName = _users.Result.FirstName,
                SurName = _users.Result.SurName,
                SecondSurName = _users.Result.SecondSurName,
                Age = _users.Result.Age,
                UserName = _users.Result.UserName,
                Email = _users.Result.Email,
                UserId = _users.Result.UserId,
                PicturePath = _users.Result.PicturefullPath,
                MyRolName = _users.Result.RolName,
                NormalizedName = _users.Result.NormalizedName,
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult ConfirmPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPassword(ConfirmPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var _users = await _userFactoryRepository.GetUserByEmailAsync(model.UserName);

                if (_users.IsSuccess == false)
                {
                    _flashMessage.Danger(_users.Message, "Incorrect information check");
                    return View(model);
                }
                var _result = await _userFactoryRepository.GetConfirmPasswordAsync(model);
                if (_result.IsSuccess)
                {
                    _flashMessage.Confirmation(_result.Message, "Correct information check");
                    return RedirectToAction("MyLoginPartial", "Account");
                }
                _flashMessage.Danger(_result.Message, "Incorrect information check");
            }
            _flashMessage.Danger("no Data.....", "Incorrect information check");
            return View(model);
        }

    }
}

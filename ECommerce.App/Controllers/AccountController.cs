using ECommerce.App.Helpers;
using ECommerce.App.Helpers.Interfaces;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.Entities;
using ECommerce.Common.Models;
using ECommerce.Common.Models.Dtos;
using ECommerce.Common.Responses;
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
        private readonly IMailHelper _mailHelper;

        public AccountController(IConfiguration configuration, IUserFactoryRepository userFactoryRepository
            , ICombosHelper combosHelper, IFlashMessage flashMessagee, ILogger<BodegasController> log, IMailHelper mailHelper)
        {
            _configuration = configuration;
            _userFactoryRepository = userFactoryRepository;
            _combosHelper = combosHelper;
            _flashMessage = flashMessagee;
            _log = log;
            _mailHelper = mailHelper;
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
        [Authorize(Roles = UserRolesResponsive.AdminSuperUser)]
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

        public IActionResult LoadaddProductPopup()
        {
            RecoverPasswordViewModel _model = new RecoverPasswordViewModel();
            try
            {
                return PartialView("_RecoverPassword", _model);
            }
            catch (Exception)
            {
                return PartialView("_RecoverPassword", _model);
            }
        }

        public IActionResult RecoverPasswordDialog()
        {
            return View(new RecoverPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecoverPasswordDialog(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var  user = await _userFactoryRepository.GetUserByEmailAsync(model.UserName);
                if (user == null)
                {
                    _flashMessage.Danger("El email no corresponde a ningún usuario registrado.");
                    return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "RecoverPasswordDialog", model) });
                }

                var _ResultToken = await _userFactoryRepository.GeneratePasswordResetTokenAsync(model);

                if (!_ResultToken.IsSuccess)
                {
                   
                    _flashMessage.Danger("El Token no corresponde a ningún usuario registrado.", _ResultToken.ErrorMessage);
                    return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "RecoverPasswordDialog", model) });
                }
                Guid activationCode = Guid.NewGuid();
                var TblResetP = new TblResetPassword
                {
                    ResetPasswordId = Guid.NewGuid(),
                    UserId = _ResultToken.Result.ObtainUser.UserId,
                    UserName = _ResultToken.Result.ObtainUser.UserName,
                    Jwt = activationCode.ToString(),
                    Token = _ResultToken.Result.Token,
                    ExpirationDate = _ResultToken.Result.Expiration.ToUniversalTime(),
                    IsDeleted = 10,
                    RegistrationDate = DateTime.Now.ToUniversalTime(),
                };
                var resultRP = await _userFactoryRepository.TBResetPasswordsAsync(TblResetP);
                if (!resultRP.IsSuccess)
                {
                    _flashMessage.Danger("Incorrect information Order Tmp check", resultRP.ErrorMessage);
                    return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "RecoverPasswordDialog", model) });
                }

                string link = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = _ResultToken.Result.Token }, protocol: HttpContext.Request.Scheme);
               
                _mailHelper.SendMail(
                    $"{user.Result.FullName.ToString()}",
                    $"{model.UserName.ToString()}",
                    "ECommerce App - Recuperación de Contraseña"+
                    $"<h1>ECommerce App - Recuperación de Contraseña</h1>" +
                    $"Para recuperar la contraseña haga click en el siguiente enlace:" +
                    $"<p><a href = \"{link}\">Reset Password</a></p>");
                _flashMessage.Info("Las instrucciones para recuperar la contraseña han sido enviadas a su correo.");
                return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "MyLoginPartial") });
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "RecoverPasswordDialog", model) });
        }

        [HttpGet]
        public IActionResult ChangingPassword()
        {
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ChangingPassword(ChangePasswordViewModel model) 
        //{
        //}

        public IActionResult ResetPassword(string Vicissitude, string UserName, string Jwt, string token)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(Jwt) || string.IsNullOrEmpty(UserName))
            {
                return new NotFoundViewResult("_ResourceNotFound");
            }
            
            var model = new ResetPasswordViewModel { Token = token, Jwt = Jwt, UserName = UserName, UserId = Vicissitude };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            Guid resultId = new Guid(model.UserId);
            var user = await _userFactoryRepository.GetToObtainUserAsync(resultId, model.UserName);
            if (user != null)
            {
                var result = await _userFactoryRepository.ResetPasswordAsync(user.Result, model.Password, model.Jwt, model.Token, model.Password);
                if (result.IsSuccess)
                {
                    ViewBag.Message = "Password reset successful.";
                    _flashMessage.Confirmation("", "Password reset successful.");
                    return RedirectToAction("MyLoginPartial", "Account");
                }

                ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            ViewBag.Message = "User not found.";
            return View(model);
        }

    }
}

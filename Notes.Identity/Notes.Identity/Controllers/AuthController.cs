using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;

namespace Notes.Identity.Controllers
{
    /*
        * AuthController - это контроллер, основная функция которого состоит в обработке действий, 
          связанных с регистрацией, входом и выходом пользователя в систему.

        * Здесь есть несколько зависимостей, которые внедряются через конструктор:

        * SignInManager<AppUser>: предоставляет API для управления входом пользователей в систему.

        * UserManager<AppUser>: предоставляет API для управления пользователями в системе.
        
        * IIdentityServerInteractionService: сервис IdentityServer, который предоставляет 
          API для взаимодействия с IdentityServer.

        * Теперь давайте более подробно разберем каждый метод:

            * Login (HttpGet): Экшн отвечает на GET-запросы по адресу /auth/login. 
              Он создает модель представления LoginViewModel с заданным URL возврата, 
              а затем передает эту модель представления представлению, чтобы отобразить форму входа.

            * Login (HttpPost): Этот экшн отвечает на POST-запросы по адресу /auth/login. 
              Он предполагает, что пользователь отправил форму входа и пытается войти в систему 
              с предоставленным именем пользователя и паролем. Если модель входящих данных 
              недействительна или пользователь не найден, экшн возвращает представлению 
              ту же форму, но с сообщением об ошибке. Если все в порядке, пользователь 
              входит в систему, а экшн перенаправляет его обратно на URL, указанный при входе.

            * Register (HttpGet): Этот экшн отвечает на GET-запросы по адресу /auth/register, 
              создает модель представления RegisterViewModel с заданным URL возврата и передает 
              эту модель представления представлению, чтобы отобразить форму регистрации.

            * Register (HttpPost): Этот экшн отвечает на POST-запросы по адресу /auth/register. 
              Он предполагает, что пользователь отправил форму регистрации и пытается зарегистрироваться 
              с предоставленным именем пользователя и паролем. Если модель входящих 
              данных недействительна, экшн возвращает представлению ту же форму, но 
              с сообщением об ошибке. Если все в порядке, для пользователя создается 
              новая учетная запись, пользователь входит в систему, а экшн перенаправляет 
              его обратно на URL, указанный при регистрации.

        Logout: Этот экшн отвечает на GET-запросы по адресу /auth/logout. Он выполняет 
        выход пользователя из системы, а затем перенаправляет его на URL, указанный в запросе на выход.
    */
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IIdentityServerInteractionService interactionService) =>
            (_signInManager, _userManager, _interactionService) =
            (signInManager, userManager, interactionService);

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return View(viewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(viewModel.Username,
                viewModel.Password, false, false);
            if (result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Login error");
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var viewModel = new RegisterViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = new AppUser
            {
                UserName = viewModel.Username
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(viewModel.ReturnUrl);
            }
            ModelState.AddModelError(string.Empty, "Error occurred");
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }
    }
}

using AirsoftMatchMaker.Core.Contracts;
using AirsoftMatchMaker.Core.Models.Users;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using AirsoftMatchMaker.Web.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AirsoftMatchMaker.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<Role> roleManager;
        private readonly IUserService userService;
        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.userService = userService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            UserRegisterModel model = new UserRegisterModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                UserName = model.Username,
                Email = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                await userManager.AddToRoleAsync(user, "GuestUser");
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty,"Username already taken!");
            return RedirectToAction(nameof(Register));
        }
        [HttpGet]
        public IActionResult Login()
        {
            UserLoginModel model = new UserLoginModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password,
                isPersistent: false,
                lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login!");
                return View(model);
            }

            return RedirectToAction(nameof(LoginRedirect));
        }

        public async Task<IActionResult> LoginRedirect()
        {
            var user = await userManager.GetUserAsync(User);
            bool isAdmin = await userManager.IsInRoleAsync(user, "Administrator");
            if (isAdmin)
            {
                return Redirect("/Administrator/Home/Index");
            }
            bool isVendor = await userManager.IsInRoleAsync(user, "Vendor");
            if (isVendor)
            {
                return Redirect("/Vendor/Home/Index");
            }
            bool isPlayer = await userManager.IsInRoleAsync(user, "Player");
            if (isPlayer)
            {
                return Redirect("/Player/Home/Index");
            }
            bool isMatchmaker = await userManager.IsInRoleAsync(user, "Matchmaker");
            if (isMatchmaker)
            {
                return Redirect("/Matchmaker/Home/Index");
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogoutAndLogin()
        {
            var user = await userManager.GetUserAsync(User);
            await signInManager.SignOutAsync();
            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "RoleRequests");
        }

        public async Task<IActionResult> MyProfile()
        {
            var model = await userService.GetCurrentUserProfileAsync(User.Id());
            return View(model);
        }
    }
}

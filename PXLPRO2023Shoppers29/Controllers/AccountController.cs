using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PXLPRO2023Shoppers29.Data;
using PXLPRO2023Shoppers29.Data.DefaultData;
using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Models.ViewModels;
using System.Security.Claims;

namespace PXLPRO2023Shoppers29.Controllers
{
    
    public class AccountController : Controller
    {
        ShopDbContext _context;
        UserManager<ShopUser> _userManager;
        SignInManager<ShopUser> _signInManager;
        public AccountController(ShopDbContext context, UserManager<ShopUser> userManager, SignInManager<ShopUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public async Task<IActionResult> LogOutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var finduser = await _userManager.FindByEmailAsync(vm.Email);
                    if (finduser == null) 
                    {
                        ModelState.AddModelError("", "This user does not exist!");
                        return View(vm);
                    }
                    
                    var result = await _signInManager.PasswordSignInAsync(finduser.UserName, vm.Password, false, false);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(finduser);
                        if (roles.Contains(Roles.Admin))
                        {
                            return RedirectToAction("BlazorProduct", "Home");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Error could not log in!");
                }
            }
            ModelState.AddModelError("", "Error could not log in!");
            return View(vm);

        }
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var customer = new Customer();

                    var user = new ShopUser();
                    user.Email = vm.Email;
                    var userName = vm.Email.Split('@');
                    user.UserName = userName[0];
                    var result = await _userManager.CreateAsync(user, vm.Password);
                    if (result.Succeeded)
                    {
                        var userFound = await _userManager.FindByEmailAsync(user.Email);
                        await _context.AddAsync(customer);
                        await _context.SaveChangesAsync();

                        userFound.CustomerId = customer.CustomerId;
                        await _userManager.UpdateAsync(userFound);
                        await _userManager.AddToRoleAsync(userFound, Roles.Client);
                        return RedirectToAction("Login", "Account");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "There has been an error!");
                    return View(vm);
                }
            }
            return View(vm);
        }
        #endregion

        #region Settings
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Settings(int? id)
        {
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            ShopUser user = null;
            if (_signInManager.IsSignedIn(User))
            {
                user = await _userManager.GetUserAsync(User);

                if (user.CustomerId == id)
                {
                    var customer = await _context.Customers.Where(x => x.CustomerId == id).FirstOrDefaultAsync();
                    if (customer == null)
                    {
                        return NotFound();
                    }

                    var orders = _context.Orders.Include(x => x.OrderLines).Where(x => x.CustomerId == id).ToList();
                    SettingsViewModel vm = new SettingsViewModel()
                    {
                        Customer = customer,
                        Orders = orders
                    };
                    return View(vm);
                }
                
            }

            return RedirectToAction("Account", "Login");


        }

        [HttpPost]
        public async Task<IActionResult> Settings(SettingsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm.Customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "There has been an error!");
                    return View(vm);
                }
            }
            return View(vm);
        }

        #endregion

        #region Facebook


        private ShopUser GetIdentityUser(ExternalLoginInfo info, UserViewModel model)
        {
            /*string userName = info.Principal.FindFirst(ClaimTypes.Name).Value;
            userName = $"{userName}_{info.LoginProvider}_{info.ProviderKey}";*/

            string userName = $"{model.UserName}_{info.LoginProvider}_{info.ProviderKey}";
            //string email = info.Principal.FindFirst(ClaimTypes.Email).Value;
            ShopUser user = new ShopUser()
            {
                UserName = userName,
                Email = model.Email
            };

            return user;
        }

        private async Task<IdentityResult> CreateIdentityUserAsync(ExternalLoginInfo externalLoginInfo, UserViewModel model)
        {
            //Put info provided by external provider (claims) into a viewmodel//Sign in failed -> user does not exist yet in our database -> create one
            Customer customer = new Customer();
            ShopUser user = GetIdentityUser(externalLoginInfo, model);
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var userFound = await _userManager.FindByEmailAsync(user.Email);
                await _context.AddAsync(customer);
                await _context.SaveChangesAsync();

                userFound.CustomerId = customer.CustomerId;
                await _userManager.UpdateAsync(userFound);
                await _userManager.AddToRoleAsync(userFound, Roles.Client);

                var identityResult = await _userManager.AddLoginAsync(user, externalLoginInfo);
                if (identityResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Description = "error in AddLogin" });
                }
            }
            
            return result;
        }
        public IActionResult FacebookLogin()
        {
            string redirectUrl = Url.Action("Response", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        #endregion

        #region Google

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin()
        {
            string redirectUrl = Url.Action("Response", "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> Response()
        {
            //retrieve information that was send in the http request (by facebook)
            ExternalLoginInfo externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                //user did not login properly with facebook -> redirect to login page
                return RedirectToAction(nameof(Login));
            }

            //Put info provided by facebook (claims) into a viewmodel
            string userName = externalLoginInfo.Principal.FindFirst(ClaimTypes.Name).Value;
            //make sure username is unique
            UserViewModel model = new UserViewModel()
            {
                UserName = userName,
                Email = externalLoginInfo.Principal.FindFirst(ClaimTypes.Email).Value
            };

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var loginProviders = await _userManager.GetLoginsAsync(user);
                if (loginProviders.Any(l => l.LoginProvider == externalLoginInfo.LoginProvider))
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error with login!");
                        return RedirectToAction("Login", "Account");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "This email already exists");
                    return RedirectToAction("Login", "Account");
                }
            }

            var identityResult = await CreateIdentityUserAsync(externalLoginInfo, model);
            if (identityResult.Succeeded)
            {

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");
        }


        #endregion

        #region Duende

        public IActionResult DuendeLogin()
        { 
            string redirectUrl = Url.Action("DuendeResponse", "Account"); 
            string scheme = "oidc"; 
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl); 
            return new ChallengeResult(scheme, properties); 
        }

        public async Task<IActionResult> DuendeResponse()
        {
            ExternalLoginInfo externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync(); 
            if (externalLoginInfo == null) 
            { 
                return RedirectToAction(nameof(Login)); 
            }

            string userName = externalLoginInfo.Principal.FindFirst("name").Value;
            UserViewModel model = new UserViewModel()
            { 
                UserName = userName, 
                Email = externalLoginInfo.Principal.FindFirst("email").Value 
            };

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider, externalLoginInfo.ProviderKey, false); 
            if (!result.Succeeded) 
            { 
                var identityResult = await CreateIdentityUserAsync(externalLoginInfo, model); 

                if (!identityResult.Succeeded) 
                { 
                   return View("Login"); 
                } 
            }

            return RedirectToAction("Index","Home");
        }
            #endregion

        }
}

using USh.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using USh.Models;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;

namespace CourseProjectDB.Areas.Customer.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;       
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(User obj) 
        {
            bool success = true;
            List<User> users = _unitOfWork.User.GetAll().ToList();
            foreach (var user in users) 
            {
                if(user.Login == obj.Login) 
                {
                    string Decodet = _unitOfWork.User.DecryptString(user.Password);
                    success = false;
                    if(obj.Password == Decodet) 
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Login),
                            new Claim(ClaimTypes.Role, user.role),
                            new Claim("UserID", user.ID.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true 
                        };

                        HttpContext.SignInAsync(
                           CookieAuthenticationDefaults.AuthenticationScheme,
                           new ClaimsPrincipal(claimsIdentity),
                           authProperties).GetAwaiter().GetResult();

                        TempData["success"] = "Ви ввійшли! 😀";
                        return Redirect("Home/Index");
                    }
                    else 
                    {
                        ModelState.AddModelError("password", "Не вірний пароль.");
                        return View();
                    }
                }
            }
            if (success) 
            {
                ModelState.AddModelError("login", "Ми не знайшли користувача із таким username");
                return View();
            }
            return View();
        }
    }
}

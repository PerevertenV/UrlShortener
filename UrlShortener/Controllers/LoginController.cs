using USh.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using USh.Models;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;
using Services.IServices;

namespace CourseProjectDB.Areas.Customer.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        public LoginController(IUnitOfWork unitOfWork, IService service)
        {
            _unitOfWork = unitOfWork;     
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(User obj) 
        {
            bool success = false;
            List<User> users = _unitOfWork.User.GetAll().ToList();
            foreach (var user in users) 
            {
                if(user.Login == obj.Login) 
                {
                    string Decodet = _service.User.DecryptString(user.Password);
                    success = true;
                    if(obj.Password == Decodet) 
                    {
                        _service.SingIn.SignInUser(user.Login, user.role, user.ID);

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
            ModelState.AddModelError("login", "Ми не знайшли користувача із таким username");  
            return View();
        }
    }
}

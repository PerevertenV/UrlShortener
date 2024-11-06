﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text.RegularExpressions;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;
using USh.Utility;

namespace UrlShortener.Controllers
{
	public class RegisterController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
        public RegisterController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;

		}

		public IActionResult Index()
		{
			Dictionary<string, string> list = new Dictionary<string, string>()
			{
				{ StaticData.Role_Admin, "Admin" },
				{ StaticData.Role_Customer, "Customer" }
			};
			IEnumerable<SelectListItem> RoleList = list.Select(u => new SelectListItem
			{
				Text = u.Value,
				Value = u.Key
			});
			ViewBag.List = RoleList;
			return View();
		}
		[HttpPost]
		public IActionResult Index(User obj, IFormCollection form)
		{
			string confirmPassword = form["confirmPassword"];
			List<User> users = _unitOfWork.User.GetAll().ToList();
			bool PasswordChecker = Regex.IsMatch(obj.Password, "[a-zA-Z]");
			foreach (User user in users)
			{
				if (obj.Login == user.Login)
				{
					ModelState.AddModelError("login", "Користувач із таким Login вже існує");
					return View();
				}
			}
			if (obj.Password.Length < 6 || obj.Password.Length > 15)
			{
				ModelState.AddModelError("password", "Пароль має містити від 5 до 15 символів");
				return View();
			}
			else if (!PasswordChecker)
			{
				ModelState.AddModelError("password", "Пароль повинен містити хоча б одну латинську літеру");
				return View();
			}
			else if (confirmPassword != obj.Password)
			{
				ModelState.AddModelError("password", "Пароль мають збігатися");
				return View();
			}
			else
			{
				string WhichRole = User.IsInRole("Admin") ? obj.role : StaticData.Role_Customer;
				var UserToAdding = new User
				{
					Login = obj.Login,
					Password = _unitOfWork.User.PasswordHashCoder(obj.Password),
					role = WhichRole
				};

				_unitOfWork.User.Add(UserToAdding);
				var UserId = _unitOfWork.User.GetFirstOrDefault(u => u.Login == obj.Login).ID;
				if (!User.Identity.IsAuthenticated)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, obj.Login),
						new Claim(ClaimTypes.Role, WhichRole),
						new Claim("UserID", UserId.ToString())
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
				}
				TempData["success"] = "Акаунт було створено успішно! 😀";
				return Redirect("Home/Index");
			}
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography.Pkcs;
using UrlShortener.Models;
using USh.DataAccess.Repository;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;
using USh.Utility;

namespace UrlShortener.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;

		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult AddCheckShortUrl(int? id) //add and check in one method
		{
			if (id != null && id != 0)
			{
				var ShortUrlFromDb = _unitOfWork.Url.GetFirstOrDefault(x => x.Id == id);
				ShortUrlFromDb.User = _unitOfWork.User.GetFirstOrDefault(x => x.ID == ShortUrlFromDb.UserWhoCreatedUrlId);
				return View(ShortUrlFromDb);
			}
			else
			{
				return View(new URL());
			}

		}
		[HttpPost]
		public IActionResult AddCheckShortUrl(URL obj)
		{
			List<string> LongUrlListFromDb = _unitOfWork.Url.GetAll()
				.Select(u => u.LongUrl).ToList();
			if (LongUrlListFromDb.Contains(obj.LongUrl))
			{
				ModelState.AddModelError("LongUrl", "Це URL уже існує");
				return View(obj);
			}

			obj.UniqueCode = _unitOfWork.Url.GenerateUniqueCode();
			obj.ShortUrl = _unitOfWork.Url.CreateShortUrl(obj.UniqueCode);
			var user = HttpContext.User;
			var userId = user.FindFirstValue("UserID");
			obj.User = _unitOfWork.User.GetFirstOrDefault(u => u.ID == int.Parse(userId));
			obj.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
			_unitOfWork.Url.Add(obj);
			TempData["success"] = "Коротке URL згенеровано успішно";
			return RedirectToAction(nameof(Index));

		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			var user = HttpContext.User;
			var userId = user.FindFirstValue("UserID");

			User userFromDb = _unitOfWork.User.GetFirstOrDefault(u => u.ID == int.Parse(userId)); ;

			List<URL> ObjectsFromDb = User.IsInRole("Admin") ? _unitOfWork.Url
				.GetAll().ToList() : _unitOfWork.Url.GetAll(u => 
					u.UserWhoCreatedUrlId == userFromDb.ID).ToList();

			return Json(new { data = ObjectsFromDb });
		}
		[HttpDelete]
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var UrlToBeDeleted = _unitOfWork.Url.GetFirstOrDefault(u => u.Id == id);
			if (UrlToBeDeleted == null)
			{
				return Json(new { success = false, message = "Помилка під час видалення" });
			}
			List<URL> urls = _unitOfWork.Url.GetAll().ToList();
			_unitOfWork.Url.Delete(UrlToBeDeleted);

			return Json(new { success = true, message = "Url видалено успішно" });
		}
		#endregion

	}
}

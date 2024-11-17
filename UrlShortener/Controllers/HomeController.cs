using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Services.IServices;
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
        private readonly IService _service;
        protected int userId;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IService service)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _service = service;
            userId = _service.User.GetUserId();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerifiUser()
        {
            return View(_service.Url.GenerateRandomTask());
        }

        public IActionResult AddCheckShortUrl(int? id) //add and check in one method
        {
            var UsersDomens = _unitOfWork.Domen.GetAll(u => u.UserId == userId);

            if (UsersDomens.Any())
            {
                IEnumerable<SelectListItem> DomensList = UsersDomens
                    .Select(u => new SelectListItem
                    {
                        Text = u.UserDomen,
                        Value = u.Id.ToString()
                    }).Concat(new List<SelectListItem>
                    {
                        new SelectListItem { Text = StaticData.ShortUrlTemplate, Value = "0" }
                    });

                ViewBag.DomenList = DomensList;
            }
            else { ViewBag.DomenList = null; }

            if (id != null && id != 0)
            {
                var ShortUrlFromDb = _unitOfWork.Url.GetFirstOrDefault(x => x.Id == id);
                
                int ShortsUrlUserId = ShortUrlFromDb.UserWhoCreatedUrlId;

                ShortUrlFromDb.User = _unitOfWork.User.GetFirstOrDefault(x => x.ID == ShortsUrlUserId);

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
            if (!obj.LongUrl.Contains("https://")) 
            {
                ModelState.AddModelError("LongUrl", "Невалідне довге посилання");
                return View(obj);
            }

            List<string> LongUrlListFromDb = _unitOfWork.Url.GetAll().Select(u => u.LongUrl).ToList();

            if (LongUrlListFromDb.Contains(obj.LongUrl))
            {
                ModelState.AddModelError("LongUrl", "Це URL уже існує");
                return View(obj);
            }

            string domen = obj.domenId == 0 ? StaticData.ShortUrlTemplate : _unitOfWork.Domen
                .GetFirstOrDefault(u => u.Id == obj.domenId).UserDomen;

            KeyValuePair<string, int> UrlAndUniqueCode = _service.Url.CreateShortUrl(domen);

            obj.UniqueCode = UrlAndUniqueCode.Value;

            obj.ShortUrl = UrlAndUniqueCode.Key;

            obj.User = _unitOfWork.User.GetFirstOrDefault(u =>
                u.ID == userId);

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

            List<URL> ObjectsFromDb = User.IsInRole("Admin") ? _unitOfWork.Url.GetAll().ToList()
                : _unitOfWork.Url.GetAll(u => u.UserWhoCreatedUrlId == int.Parse(userId)).ToList();

            return Json(new { data = ObjectsFromDb });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var UrlToBeDeleted = _unitOfWork.Url.GetFirstOrDefault(u => u.Id == id);
            if (UrlToBeDeleted == null)
            {
                return Json(new { success = false, message = "Помилка під час видалення" });
            }

            _unitOfWork.Url.Delete(UrlToBeDeleted);

            return Json(new { success = true, message = "Url видалено успішно" });
        }
        #endregion

    }
}

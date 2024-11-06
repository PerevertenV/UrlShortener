using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using USh.DataAccess.Repository;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;

namespace UrlShortener.Controllers
{
    public class DomenController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DomenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
                
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddDomen() 
        {
            return View(new Domen());
        }
        [HttpPost]
        public IActionResult AddDomen(Domen domen) 
        {
            var CheckDomens = _unitOfWork.Domen.GetAll(u => u.UserDomen == domen.UserDomen).ToList();

            if (CheckDomens.Any())
            {
				ModelState.AddModelError("UserDomen", "Цей домен уже існує");
                return View();
			}

            var user = HttpContext.User;
            var userId = user.FindFirstValue("UserID");

            domen.UserId = int.Parse(userId);
            domen.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

            _unitOfWork.Domen.Add(domen);
            TempData["success"] = "Домен доданий успішно";
			return RedirectToAction(nameof(Index));
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var user = HttpContext.User;
            var userId = user.FindFirstValue("UserID");

            User userFromDb = _unitOfWork.User.GetFirstOrDefault(u => u.ID == int.Parse(userId));

            List<Domen> ObjectsFromDb = User.IsInRole("Admin") ? _unitOfWork.Domen
                .GetAll().ToList() : _unitOfWork.Domen.GetAll(u =>
                    u.UserId == userFromDb.ID).ToList();

            return Json(new { data = ObjectsFromDb });
        }
        [HttpDelete]
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var DomenToBeDeleted = _unitOfWork.Domen.GetFirstOrDefault(u => u.Id == id);
            if (DomenToBeDeleted == null)
            {
                return Json(new { success = false, message = "Помилка під час видалення" });
            }

            List<Domen> urls = _unitOfWork.Domen.GetAll().ToList();
            _unitOfWork.Domen.Delete(DomenToBeDeleted);

            List<URL> urlsFromDB = _unitOfWork.Url.GetAll(u => u.domenId == id).ToList();
            if (urlsFromDB.Any())
            {
                foreach (URL url in urlsFromDB)
                {
                    _unitOfWork.Url.Delete(url);
                }
            }

            return Json(new { success = true, message = "Url видалено успішно" });
        }
        #endregion
    }
}

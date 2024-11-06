using Microsoft.AspNetCore.Mvc;
using Services.IServices;
using System.Security.Claims;
using USh.DataAccess.Repository;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;

namespace UrlShortener.Controllers
{
    public class DomenController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IService _service;
        protected int userId;
        public DomenController(IUnitOfWork unitOfWork, IService service)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            userId = _service.User.GetUserId();
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

            domen.UserId = userId;
            domen.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

            _unitOfWork.Domen.Add(domen);
            TempData["success"] = "Домен доданий успішно";
			return RedirectToAction(nameof(Index));
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Domen> ObjectsFromDb = User.IsInRole("Admin") ? _unitOfWork.Domen.GetAll().ToList() 
                : _unitOfWork.Domen.GetAll(u => u.UserId == userId).ToList();

            return Json(new { data = ObjectsFromDb });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var DomenToBeDeleted = _unitOfWork.Domen.GetFirstOrDefault(u => u.Id == id);
            if (DomenToBeDeleted == null)
            {
                return Json(new { success = false, message = "Помилка під час видалення" });
            }
            _unitOfWork.Domen.Delete(DomenToBeDeleted);

            List<URL> urlsFromDB = _unitOfWork.Url.GetAll(u => u.domenId == id).ToList();
            if (urlsFromDB.Any())
            {
                foreach (URL url in urlsFromDB)
                {
                    _unitOfWork.Url.Delete(url);
                }
            }

            return Json(new { success = true, message = "Домен видалено успішно" });
        }
        #endregion
    }
}

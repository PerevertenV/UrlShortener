using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;

namespace CourseProjectDB.Areas.Admin.Controllers
{
    [Authorize]
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<User> ListOfObjects = _unitOfWork.User.GetAll().ToList();
            return View(ListOfObjects);
        }

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<User> ObjectsFromDb = _unitOfWork.User.GetAll().ToList();
			return Json(new { data = ObjectsFromDb });
		}
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var UserToBeDeleted = _unitOfWork.User.GetFirstOrDefault(u => u.ID == id);
			if (UserToBeDeleted == null)
			{
				return Json(new { succes = false, message = "Помилка під час видалення" });
			}

			List<URL> urlsFromDB = _unitOfWork.Url.GetAll(u => u.UserWhoCreatedUrlId == id).ToList();
			List<Domen> domenFromDB = _unitOfWork.Domen.GetAll(u => u.UserId == id).ToList();

			if (urlsFromDB.Any())
			{
				foreach (URL url in urlsFromDB)
				{
					_unitOfWork.Url.Delete(url);
				}
			}
			if (domenFromDB.Any())
			{
				foreach (Domen domens in domenFromDB)
				{
					_unitOfWork.Domen.Delete(domens);
				}
			}
			_unitOfWork.User.Delete(UserToBeDeleted);


			return Json(new { succes = true, message = "Користувача було видалено успішно!" });
		}
		#endregion
	}
}

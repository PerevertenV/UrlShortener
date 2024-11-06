using Microsoft.AspNetCore.Mvc;
using System.Linq;
using USh.Utility;

namespace UrlShortener.Controllers
{
	public class UserVerificationController : Controller
	{
		public IActionResult Index()
		{
			Random rand = new Random();
			int randValue = rand.Next(2, 8);

			foreach(var task in StaticData.VerifyDict) 
			{
				if (task.Key.Contains(randValue.ToString())) 
				{
					return View(task);
				}
			}

			return View();
			
		}
	}
}

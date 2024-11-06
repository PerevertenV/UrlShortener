using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.DataAccess.Data;
using USh.Models.Models;
using USh.Utility;

namespace Services
{
    internal class UrlService : IUrlService
    {
        private ApplicationDbContext _context;

        public UrlService(ApplicationDbContext context)
        {
            _context = context;
        }

        public KeyValuePair<string, int> CreateShortUrl(string domen)
        {
            Random random = new Random();
            //згенерували випадкове число
            int RandomValue = random.Next(101, 10001);
            //отримали із БД стовпець із короткими Url для повірняння та генерації унікальності
            List<string> ShortUrlsFromDb = _context.URL.Select(u => u.ShortUrl).ToList();
            //проходимо перевірку, обираємо ітерацій скільки і елементів
            //у спику в разі відсутнсті елемента виходимо із циклу
            for (int i = 0; i < ShortUrlsFromDb.Count; i++)
            {
                if (ShortUrlsFromDb.Contains(domen + RandomValue))
                {
                    RandomValue = random.Next(101, 10001);
                }
                else { break; }
            }
            return new KeyValuePair<string, int>(domen + '/' + RandomValue.ToString(), RandomValue);
        }
    }
}

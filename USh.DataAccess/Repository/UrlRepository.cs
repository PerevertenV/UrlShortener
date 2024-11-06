using USh.DataAccess.Data;
using USh.DataAccess.Repository.IRepository;
using USh.Models.Models;
using USh.Utility;

namespace USh.DataAccess.Repository
{
    public class UrlRepository : Repository<URL>, IUrlRepository
    {
        private ApplicationDbContext _context;
        

        public UrlRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public string CreateShortUrl(int UniqueValue, string domen)
        {
            //повертаємо коротке URL шаблон та конкатенація із унікальним номером
            return domen + '/' + UniqueValue.ToString();
        }

        public int GenerateUniqueCode()
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
                if (ShortUrlsFromDb.Contains(StaticData.ShortUrlTemplate + RandomValue)) 
                {
                    RandomValue = random.Next(101, 10001);
                }
                else { break; }
            }
            return RandomValue;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Moq;
using Services;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USh.DataAccess.Data;
using USh.Models.Models;

namespace UnitTests
{
    internal class unitTestForUrl
    {

        private ApplicationDbContext _context;
        private Mock<IUrlService> _urlServiceMock;


        [SetUp]
        public void SetUp()
        {
            // Налаштовуємо InMemoryDatabase для тестового контексту
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            _urlServiceMock = new Mock<IUrlService>();

            //Налаштування моків для _urlServiceMock
            _urlServiceMock
                .Setup(service => service.CreateShortUrl(It.IsAny<string>()))
                .Returns((string domen) =>
                {
                    Random random = new Random();
                    int RandomValue = random.Next(101, 10001);
                    return new KeyValuePair<string, int>(domen + '/' + RandomValue.ToString(), RandomValue);
                });

            // Наповнюємо базу тестовими даними
            _context.URL.AddRange(new List<URL>
            {
                new URL { ShortUrl = "domen1/123", LongUrl = "1" },
                new URL { ShortUrl = "domen2/456", LongUrl = "1" },
                new URL { ShortUrl = "domen3/789", LongUrl = "1" }
            });

            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            // Очищаємо базу після кожного тесту
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        [TestCase("domen1")]
        [TestCase("domen2")]
        [TestCase("domen3")]
        public void CheckCorrect_UrlGenaration(string domen)
        {
            // Діємо
            var result = _urlServiceMock.Object.CreateShortUrl(domen);

            // Перевірка, що згенерований URL містить домен
            Assert.That(result.Key, Does.Contain(domen));
        }
    }

}


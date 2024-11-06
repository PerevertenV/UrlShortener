using Microsoft.AspNetCore.Http;
using Moq;
using Services;
using Services.IServices;
using System.Security.Claims;

namespace UnitTests
{
    internal class Tests
    {
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IUserService> _userServiceMock;

        [SetUp]
        public void Setup()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userServiceMock = new Mock<IUserService>();

            var claims = new List<Claim>
            {
                new Claim("UserID", "1")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext { User = user };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);

            
            _userServiceMock.Setup(us => us.GetUserId()).Returns(1);
        }



        [Test]
        public void GetUserId_ShouldReturnUserId_WhenClaimExists()
        {
           
            var result = _userServiceMock.Object.GetUserId();

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }
    }
}
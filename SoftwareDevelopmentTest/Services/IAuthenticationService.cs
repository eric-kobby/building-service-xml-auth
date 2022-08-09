using SoftwareDevelopmentTest.Models;

namespace SoftwareDevelopmentTest.Services
{
    public interface IAuthenticationService
    {
        AuthHeaderValue GetAuthHeaderValues();
    }
}
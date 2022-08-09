using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using SoftwareDevelopmentTest.Models;

namespace SoftwareDevelopmentTest.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly Credential _credential;
        public AuthenticationService(IOptions<Credential> credentialOption)
        {
            _credential = credentialOption.Value;
        }

        public AuthHeaderValue GetAuthHeaderValues()
        {
            using var cryptoProvider = SHA1.Create();
            var timeStamp = DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalMilliseconds;
            var buffer = Encoding.UTF8.GetBytes($"{_credential.Username}{_credential.ApiKey}{timeStamp}");
            var hashBuffer = cryptoProvider.ComputeHash(buffer);
            return new AuthHeaderValue
            {
                TimeStamp = $"{timeStamp}",
                Token = BitConverter.ToString(hashBuffer).Replace("-","").ToLower(),
                APIkey = _credential.Username
            };
        }
    }
}


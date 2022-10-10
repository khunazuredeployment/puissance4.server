using Puissance4.Business.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Puissance4.Infrastructure.Services
{
    public class HashService: IHashService
    {
        public byte[] HashPassword(string plainPassword, Guid salt)
        {
            return SHA512.HashData(Encoding.UTF8.GetBytes(plainPassword + salt.ToString()));
        }

        public bool VerifyPassword(byte[] hashedPassword, string plainPassword, Guid salt)
        {
            return hashedPassword.SequenceEqual(HashPassword(plainPassword, salt));
        }
    }
}

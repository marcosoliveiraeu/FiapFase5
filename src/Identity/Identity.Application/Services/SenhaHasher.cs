using Identity.Application.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Application.Services
{
    public class SenhaHasher : ISenhaHasher
    {
        public string Hash(string senha)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public bool Verify(string senha, string hash)
        {
            var hashDaSenha = Hash(senha);
            return hashDaSenha == hash;
        }


    }
}

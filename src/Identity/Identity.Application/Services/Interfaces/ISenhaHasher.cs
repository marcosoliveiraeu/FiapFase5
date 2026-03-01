
namespace Identity.Application.Services.Interfaces
{
    public interface ISenhaHasher
    {
        string Hash(string senha);
        bool Verify(string senha, string hash);
    }
}

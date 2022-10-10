namespace Puissance4.Business.Interfaces
{
    public interface IHashService
    {
        byte[] HashPassword(string plainPassword, Guid salt);
        bool VerifyPassword(byte[] hashedPassword, string plainPassword, Guid salt);
    }
}

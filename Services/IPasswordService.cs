public interface IPasswordService
{
    string HashPassword(string password);
    string HashPasswordWithSalt(string password, string salt);
    bool VerifyPassword(string inputPassword, string storedHash);
    string GenerageRandomPassword(int length);
}

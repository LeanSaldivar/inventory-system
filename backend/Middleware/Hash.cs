namespace backend.middleware;

/// <summary>
/// Service for hashing and verifying passwords using bcrypt
/// </summary>
public class Hash : IHash
{
    /// <summary>
    /// Hashes a password using bcrypt
    /// </summary>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    }

    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}

public interface IHash
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
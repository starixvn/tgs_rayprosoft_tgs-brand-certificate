using Tgs.Core.Infrastructures.Security;

namespace Tgs.BrandCertificate;

public interface ICredentialGenerator
{
    string GetApiKey();

    string GetSecretKey();
}

public class CredentialGenerator(IPasswordHandler password) : ICredentialGenerator
{
    private readonly IPasswordHandler _password = password;

    string ICredentialGenerator.GetApiKey() => GetRandomStringByLength(40);

    string ICredentialGenerator.GetSecretKey() => GetRandomStringByLength(20);

    private string GetRandomStringByLength(ushort length) => _password.GenerateRandomSalt(length).ToBase62();
}

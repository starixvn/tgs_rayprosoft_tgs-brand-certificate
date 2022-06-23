namespace Tgs.BrandCertificate.ApplicationServices;

public record MakeBrandTokenCommand(Guid BrandId, string Code);

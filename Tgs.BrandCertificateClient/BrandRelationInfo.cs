namespace Tgs.BrandCertificate.Clients;

public record BrandRelationInfo(
	Guid BrandId,
	string BrandCode,
	string BrandName,
	Guid LicenseeId,
	string LicenseeCode,
	string LicenseeName,
	Guid ResellerId,
	string ResellerCode,
	string ResellerName);
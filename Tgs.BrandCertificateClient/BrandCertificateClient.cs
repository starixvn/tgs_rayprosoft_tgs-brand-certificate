using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Tgs.BrandCertificate.Clients;

internal class BrandCertificateClient : IBrandCertificateClient
{
	private readonly HttpClient _httpClient;

	public BrandCertificateClient(HttpClient httpClient)
	{
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
	}

	public async Task<string?> MakeJsonWebTokenAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var requestContent = new StringContent(
			string.Empty,
			Encoding.UTF8,
			MediaTypeNames.Application.Json);

		var response = await _httpClient
			.PostAsync($"/cert/{brandId}", requestContent, cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		if (response.StatusCode == HttpStatusCode.NoContent)
			return null;

		var jwtJson = await response.Content
			.ReadAsStringAsync(cancellationToken)
			.ConfigureAwait(false);

		return jwtJson;
	}

	public async Task<SecurityKey> GetSystemValidateSecurityKeyAsync(CancellationToken cancellationToken = default)
	{
		var jwk = await GetSystemJsonWebKeyAsync(cancellationToken).ConfigureAwait(false);

		var jwks = new JsonWebKeySet();
		jwks.Keys.Add(jwk);

		return jwks.GetSigningKeys().First();
	}

	private async Task<JsonWebKey> GetSystemJsonWebKeyAsync(CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
			.GetAsync("/cert/jwk", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var jwkJson = await response.Content
			.ReadAsStringAsync(cancellationToken)
			.ConfigureAwait(false);

		return new JsonWebKey(jwkJson);
	}
}

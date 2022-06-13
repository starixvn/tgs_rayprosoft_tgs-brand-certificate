using System.Text.Json;

namespace Tgs.BrandCertificate.Clients;

internal class BrandServiceClient : IBrandServiceClient
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true
	};

	private readonly HttpClient _httpClient;

	public BrandServiceClient(HttpClient httpClient)
	{
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
	}

	public async ValueTask<BrandInfo?> GetBrandInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
			.GetAsync($"/brand/{brandId}", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var json = await response.Content
			.ReadAsStringAsync(cancellationToken)
			.ConfigureAwait(false);

		return JsonSerializer.Deserialize<BrandInfo>(json, _SerializerOptions);
	}
}

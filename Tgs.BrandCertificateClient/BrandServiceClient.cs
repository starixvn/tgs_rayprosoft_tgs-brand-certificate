using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Tgs.BrandCertificate.Clients;

internal class BrandServiceClient : IBrandServiceClient
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		Converters = { new JsonStringEnumConverter() }
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

	public async ValueTask<BrandFrontendInfo> GetBrandFrontendInfoAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
			.GetAsync($"/brand/front/{brandId}", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var json = await response.Content
			.ReadAsStringAsync(cancellationToken)
			.ConfigureAwait(false);

		return JsonSerializer.Deserialize<BrandFrontendInfo>(json, _SerializerOptions)!;
	}

	public async ValueTask<BrandStatus> GetBrandStatusAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
			.GetAsync($"/brand/status/{brandId}", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var json = await response.Content
			.ReadAsStringAsync(cancellationToken)
			.ConfigureAwait(false);

		return JsonSerializer.Deserialize<BrandStatus>(json, _SerializerOptions)!;
	}

	public async IAsyncEnumerable<VipLevelInfo> GetVipLevelInfosAsync(
		Guid brandId,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
			.GetAsync($"/viplevel/{brandId}", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var vipLevels = await response.Content
			.ReadFromJsonAsync<VipLevelInfo[]>(_SerializerOptions, cancellationToken)
			.ConfigureAwait(false) ?? Array.Empty<VipLevelInfo>();

		foreach (var vipLevel in vipLevels)
			yield return vipLevel;
	}

	public async ValueTask<BrandMaintenanceInfos> GetBrandMaintenanceInfosAsync(
		string brandCode,
		Paging paging,
		CancellationToken cancellationToken = default)
	{
		var queryStrings = HttpUtility.ParseQueryString(string.Empty);

		if (!string.IsNullOrWhiteSpace(brandCode))
			queryStrings.Add("brandcode", brandCode);

		if (paging.ShowAll == true)
			queryStrings.Add("all", "true");
		else
		{
			queryStrings.Add("page", paging.Page.ToString());
			queryStrings.Add("rows", paging.Rows.ToString());
		}

		if (!string.IsNullOrWhiteSpace(paging.SortBy))
		{
			queryStrings.Add("sort", paging.SortBy);

			if (paging.IsDesc == true)
				queryStrings.Add("desc", "true");
		}

		var response = await _httpClient
			.GetAsync($"/brand/maintenance?{queryStrings}", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var json = await response.Content
			.ReadAsStringAsync(cancellationToken)
			.ConfigureAwait(false);

		return JsonSerializer.Deserialize<BrandMaintenanceInfos>(json, _SerializerOptions)!;
	}

	public async ValueTask BatchUpdateBrandMaintenanceTimeAsync(
		Guid[] brandIds,
		TimeOnly beginTime,
		TimeOnly endTime,
		CancellationToken cancellationToken = default)
	{
		var dict = new Dictionary<string, object>
		{
			{ "brandids", brandIds },
			{ "begintime",beginTime.ToString("HH:mm:ss")},
			{ "endtime", endTime.ToString("HH:mm:ss") }
		};

		var requestContent = new StringContent(
			JsonSerializer.Serialize(dict, _SerializerOptions));

		requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

		var response = await _httpClient
			.PutAsync(
			   "/brand/maintenance/batchupdatetime",
				requestContent,
				cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask<BrandMaintenanceResult> GetBrandMaintenanceInfoByIdAsync(
		Guid brandMaintenanceId,
		CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
			.GetAsync($"/brand/maintenance/{brandMaintenanceId}", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var json = await response.Content
			.ReadAsStringAsync(cancellationToken)
			.ConfigureAwait(false);

		return JsonSerializer.Deserialize<BrandMaintenanceResult>(json, _SerializerOptions)!;
	}

	public async ValueTask UpdateBrandMaintenanceInfoAsync(Guid brandMaintenanceId, string Content, CancellationToken cancellationToken = default)
	{
		var dict = new Dictionary<string, object>
		{
			{ "content", Content }
		};

		var requestContent = new StringContent(
			JsonSerializer.Serialize(dict, _SerializerOptions));

		requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

		var response = await _httpClient
			.PutAsync(
			   $"/brand/maintenance/{brandMaintenanceId}",
				requestContent,
				cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}
}
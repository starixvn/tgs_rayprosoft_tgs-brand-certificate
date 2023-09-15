using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using DateOnlyTimeOnly.AspNet.Converters;
using Microsoft.AspNetCore.Http.Extensions;

namespace Tgs.BrandCertificate.Clients;

internal class BrandServiceClient : IBrandServiceClient
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		Converters = {
			new JsonStringEnumConverter(),
			new TimeOnlyJsonConverter()
		}
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

		return string.IsNullOrEmpty(json)
			? null
			: JsonSerializer.Deserialize<BrandInfo>(json, _SerializerOptions);
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

	[Obsolete("淘汰")]
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

	public async IAsyncEnumerable<BrandMaintenanceInfo> GetBrandMaintenanceInfosAsync(
		string brandCode,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var queryStrings = HttpUtility.ParseQueryString(string.Empty);

		if (!string.IsNullOrWhiteSpace(brandCode))
			queryStrings.Add("brandcode", brandCode);

		var response = await _httpClient
			.GetAsync($"/brand/maintenance?{queryStrings}", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var json = await response.Content
			.ReadAsStringAsync(cancellationToken)
			.ConfigureAwait(false);

		var brandInfos = JsonSerializer.Deserialize<BrandMaintenanceInfo[]>(json, _SerializerOptions)!;

		foreach (var bankInfo in brandInfos!)
			yield return bankInfo;
	}

	public async ValueTask UpdateAllBrandMaintenanceTimeAsync(
		TimeOnly beginTime,
		TimeOnly endTime,
		CancellationToken cancellationToken = default)
	{
		var dict = new Dictionary<string, object>
		{
			{ "begintime",beginTime},
			{ "endtime", endTime }
		};

		var requestContent = new StringContent(
			JsonSerializer.Serialize(dict, _SerializerOptions));

		requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

		var response = await _httpClient
			.PutAsync(
			   "/brand/maintenance/updatealltime",
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

	public async ValueTask UpdateBrandMaintenanceContentAsync(Guid brandMaintenanceId, string? content, CancellationToken cancellationToken = default)
	{
		var dict = new Dictionary<string, object?>
		{
			{ "content", content }
		};

		var requestContent = new StringContent(
			JsonSerializer.Serialize(dict, _SerializerOptions));

		requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

		var response = await _httpClient
			.PutAsync(
			   $"/brand/maintenance/content/{brandMaintenanceId}",
				requestContent,
				cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask<BrandPaymentSettings?> GetBrandPaymentSettingsAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient
			.GetAsync($"/brand/payment/{brandId}", cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		if (response.StatusCode == HttpStatusCode.NoContent)
			return null;

		var settings = await response.Content
			.ReadFromJsonAsync<BrandPaymentSettings>(_SerializerOptions, cancellationToken)
			.ConfigureAwait(false);

		return settings;
	}

	public async IAsyncEnumerable<Guid> GetBrandIdsByTimeSpanMinutesAsync(int timeSpanMinutes,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync(
				$"/brand/id/{timeSpanMinutes}",
				cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();

		var brandIds = await response.Content
			.ReadFromJsonAsync<Guid[]>(_SerializerOptions, cancellationToken)
			.ConfigureAwait(false) ?? Array.Empty<Guid>();

		foreach (var brandId in brandIds!)
			yield return brandId;
	}

	public IAsyncEnumerable<BrandListItem> QueryBrandListAsync(
		BrandListQuery query,
		CancellationToken cancellationToken = default)
	{
		var queryStrings = new QueryBuilder();

		if (!string.IsNullOrEmpty(query.Keyword))
			queryStrings.Add("keyword", query.Keyword);

		if (query.Status is not null)
			queryStrings.Add("status", query.Status.Value.ToString());

		foreach (var brandId in query.BrandIds)
			queryStrings.Add("brandId", brandId.ToString());

		return _httpClient
			.GetAsync($"/brand{queryStrings.ToQueryString().Value}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<BrandListItem>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.Where(arr => arr is not null)
			.SelectMany(arr => arr!.ToAsyncEnumerable())
			.ToArrayAsync(cancellationToken)
			.AsTask()
			.ToAsyncEnumerable()
			.SelectMany(arr => arr.ToAsyncEnumerable());
	}

	public IAsyncEnumerable<BrandRelationInfo> GetBrandRelationsAsync(CancellationToken cancellationToken = default)
		=> _httpClient
			.GetAsync("/brand/relations", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<BrandRelationInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.Where(arr => arr is not null)
			.SelectMany(arr => arr!.ToAsyncEnumerable())
			.ToArrayAsync(cancellationToken)
			.AsTask()
			.ToAsyncEnumerable()
			.SelectMany(arr => arr.ToAsyncEnumerable());

	public async ValueTask UpdateBrandStatusAsync(
		Guid brandId,
		BrandStatus status,
		string modifier,
		CancellationToken cancellationToken = default)
	{
		var requestContent = new StringContent(string.Empty);
		requestContent.Headers.Add("Modifier", modifier);

		var response = await _httpClient.PutAsync(
			$"/brand/status/{brandId}/{status}",
			requestContent,
			cancellationToken).ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask UpdateBrandMaintenanceTimeAsync(Guid brandId, TimeOnly beginTime, TimeOnly endTime, CancellationToken cancellationToken = default)
	{
		var dict = new Dictionary<string, object>
		{
			{ "begintime", beginTime },
			{ "endTime", endTime }
		};

		var requestContent = new StringContent(
			JsonSerializer.Serialize(dict, _SerializerOptions));

		requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(MediaTypeNames.Application.Json);

		var response = await _httpClient
			.PutAsync(
			   $"/brand/maintenance/time/{brandId}",
				requestContent,
				cancellationToken)
			.ConfigureAwait(false);

		_ = response.EnsureSuccessStatusCode();
	}

	public async ValueTask<BrandInfo?> GetBrandInfoByCodeAsync(string brandCode, CancellationToken cancellationToken = default)
		=> await _httpClient.GetAsync($"/brand/{brandCode}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<BrandInfo>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.Where(info => info is not null)
			.Select(info => info!)
			.FirstOrDefaultAsync(cancellationToken: cancellationToken)
			.ConfigureAwait(false);

	public IAsyncEnumerable<BrandCurrencyInfo> GetAllBrandCurrenciesAsync(CancellationToken cancellationToken = default)
		=> _httpClient.GetAsync("/brand/currencies", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IAsyncEnumerable<BrandCurrencyInfo>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!));

	public ValueTask<SmsVendorInfo<TSmsSettings>?> GetBrandSmsSettingsAsync<TSmsSettings>(
		Guid brandId,
		JsonSerializerOptions? jsonSerializerOptions = null,
		CancellationToken cancellationToken = default)
		=> _httpClient.GetAsync($"/brand/sms/{brandId}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<SmsVendorInfo<TSmsSettings>>(jsonSerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.FirstOrDefaultAsync(cancellationToken: cancellationToken);
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.BrandCertificateService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificateService.Controllers;

[Route("api/social")]
[ApiController]
public class SocialMediaController : ControllerBase
{
	[HttpPut("{brandId:Guid}", Name = "AddSocialMediaSetting")]
	public ValueTask<Guid> AddSocialMediaSettingAsync(
		[FromServices] IGetService<AddSocialMediaSettingCommand, Guid> createService,
		[FromHeader(Name = "Modifier")] string modifier,
		Guid brandId,
		[FromBody] AddSocialMediaSettingViewModel viewModel)
	{
		var command = new AddSocialMediaSettingCommand(
			brandId,
			viewModel.Name,
			viewModel.Type,
			viewModel.Url,
			modifier);

		return createService.GetAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("{brandId:guid}/{settingId:Guid}", Name = "DeleteSocialMediaSetting")]
	public ValueTask DeleteSocialMediaSettingAsync(
		[FromServices] ICommandService<DeleteSocialMediaSettingCommand> deleteService,
		Guid brandId,
		Guid settingId)
	{
		var command = new DeleteSocialMediaSettingCommand(
			brandId,
			settingId);

		return deleteService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpGet("{brandId:Guid}", Name = "ListSocialMediaSettings")]
	public IAsyncEnumerable<SocialMediaSettingViewModel> ListSocialMediaSettingsAsync(
		[FromServices] IQueryService<ListBrandSocialMediaSettingsQuery, SocialMediaSetting> queryService,
		Guid brandId)
	{
		var query = new ListBrandSocialMediaSettingsQuery(brandId);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(setting => new SocialMediaSettingViewModel
			{
				Id = setting.Id,
				Name = setting.Name,
				Type = setting.Type,
				Url = setting.Url
			});
	}
}

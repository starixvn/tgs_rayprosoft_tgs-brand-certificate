using Microsoft.AspNetCore.Mvc;
using Tgs.BrandCertificate.ApplicationServices;
using Tgs.BrandCertificateService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificateService.Controllers;

[Route("brand/player-contract")]
[ApiController]
public class BrandPlayerContractController : ControllerBase
{
	[HttpGet("{brandId:guid}", Name = "ListBrandPlayerContractDisplay")]
	public IAsyncEnumerable<BrandPlayerContractDisplayViewModel> ListBrandPlayerContractDisplayAsync(
		[FromServices] IQueryService<ListBrandPlayerContractDisplayQuery, BrandPlayerContractDisplayInfo> queryService,
		Guid brandId)
		=> queryService.QueryAsync(new ListBrandPlayerContractDisplayQuery(brandId), HttpContext.RequestAborted)
			.Select(info => new BrandPlayerContractDisplayViewModel
			{
				Name = info.Name,
				IsVisualized = info.IsVisualized
			});

	[HttpPatch("{brandId:guid}", Name = "SetBrandPlayerContractDisplay")]
	public ValueTask SetBrandPlayerContractDisplayAsync(
		[FromServices] ICommandService<SetBrandPlayerContractDisplayCommand> commandService,
		[FromHeader(Name = "Modifier")] string modifier,
		[FromBody] SetBrandPlayerContractDisplayRequest request,
		Guid brandId)
		=> commandService.ExecuteAsync(new SetBrandPlayerContractDisplayCommand(brandId, request.Name, request.IsVisualized, modifier));
}
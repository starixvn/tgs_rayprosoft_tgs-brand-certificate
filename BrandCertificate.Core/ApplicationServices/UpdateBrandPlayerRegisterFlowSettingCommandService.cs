using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandPlayerRegisterFlowSettingCommandService : ICommandService<UpdateBrandPlayerRegisterFlowSettingCommand>
{
	private readonly ITgsRepository _tgsRepository;
	private readonly ICommandService<RemoveBrandFrontendInfoCachedCommand> _removeCachedService;

	public UpdateBrandPlayerRegisterFlowSettingCommandService(
		ITgsRepository tgsRepository,
		ICommandService<RemoveBrandFrontendInfoCachedCommand> removeCachedService)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		_removeCachedService = removeCachedService ?? throw new ArgumentNullException(nameof(removeCachedService));
	}

	public async ValueTask ExecuteAsync(UpdateBrandPlayerRegisterFlowSettingCommand command, CancellationToken cancellationToken = default)
	{
		await _tgsRepository
			.UpdateBrandPlayerRegisterFlowSettingAsync(command.BrandId, command.PlayerRegisterFlowSettings, cancellationToken)
			.ConfigureAwait(false);

		await _removeCachedService
			.ExecuteAsync(new RemoveBrandFrontendInfoCachedCommand(command.BrandId), cancellationToken)
			.ConfigureAwait(false);
	}
}
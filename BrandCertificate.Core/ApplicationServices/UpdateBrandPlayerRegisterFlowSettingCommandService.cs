using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices
{
	internal class UpdateBrandPlayerRegisterFlowSettingCommandService : ICommandService<UpdateBrandPlayerRegisterFlowSettingCommand>
	{
		private readonly ITgsRepository _tgsRepository;
		private readonly ICommandService<RemoveCachedBrandFrontendInfoCommand> _removeCacheService;

		public UpdateBrandPlayerRegisterFlowSettingCommandService(
			ITgsRepository tgsRepository,
			ICommandService<RemoveCachedBrandFrontendInfoCommand> removeCacheService)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
			_removeCacheService = removeCacheService ?? throw new ArgumentNullException(nameof(removeCacheService));
		}

		public async ValueTask ExecuteAsync(UpdateBrandPlayerRegisterFlowSettingCommand command, CancellationToken cancellationToken = default)
		{
			await _tgsRepository
				.UpdateBrandPlayerRegisterFlowSettingAsync(command.BrandId, command.PlayerRegisterFlowSettings, cancellationToken)
				.ConfigureAwait(false);

			await _removeCacheService
				.ExecuteAsync(new RemoveCachedBrandFrontendInfoCommand(command.BrandId), cancellationToken)
				.ConfigureAwait(false);
		}
	}
}
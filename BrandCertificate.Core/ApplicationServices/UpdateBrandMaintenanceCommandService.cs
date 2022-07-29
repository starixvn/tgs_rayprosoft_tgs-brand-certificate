using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices
{
	internal class UpdateBrandMaintenanceCommandService : ICommandService<UpdateBrandMaintenanceCommand>
	{
		private readonly ITgsRepository _tgsRepository;

		public UpdateBrandMaintenanceCommandService(ITgsRepository tgsRepository)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		}

		public ValueTask ExecuteAsync(UpdateBrandMaintenanceCommand command, CancellationToken cancellationToken = default)
			=> _tgsRepository.UpdateBrandMaintenanceInfoAsync(command.BrandMaintenanceId, command.Content, cancellationToken);
	}
}
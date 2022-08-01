using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices
{
	internal class UpdateBrandMaintenanceTimeCommandService : ICommandService<UpdateBrandMaintenanceTimeCommand>
	{
		private readonly ITgsRepository _tgsRepository;

		public UpdateBrandMaintenanceTimeCommandService(ITgsRepository tgsRepository)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		}

		public ValueTask ExecuteAsync(UpdateBrandMaintenanceTimeCommand command, CancellationToken cancellationToken = default)
			=> _tgsRepository.UpdateBrandMaintenanceTimeAsync(command.BrandId, command.BeginTime, command.EndTime, cancellationToken);
	}
}
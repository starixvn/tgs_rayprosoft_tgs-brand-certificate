using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices
{
	internal class BatchUpdateBrandMaintenanceCommandService : ICommandService<BatchUpdateBrandMaintenanceCommand>
	{
		private readonly ITgsRepository _tgsRepository;

		public BatchUpdateBrandMaintenanceCommandService(ITgsRepository tgsRepository)
		{
			_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		}

		public ValueTask ExecuteAsync(BatchUpdateBrandMaintenanceCommand command, CancellationToken cancellationToken = default)
			=> _tgsRepository.BatchUpdateBrandMaintenanceTimeAsync(command.BrandIds, command.BeginTime, command.EndTime);
	}
}
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandMaintenanceTimeCommandService(ITgsRepository tgsRepository)
	: ICommandService<UpdateBrandMaintenanceTimeCommand>
{
	public ValueTask ExecuteAsync(
		UpdateBrandMaintenanceTimeCommand command,
		CancellationToken cancellationToken = default)
		=> tgsRepository.UpdateBrandMaintenanceTimeAsync(
			command.BrandId,
			command.BeginTime,
			command.EndTime,
			cancellationToken);
}

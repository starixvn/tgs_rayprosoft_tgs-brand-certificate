using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandAgentDividendTimeIntervalCommandService(ITgsRepository tgsRepository)
	: ICommandService<UpdateBrandAgentDividendTimeIntervalCommand>
{
	public ValueTask ExecuteAsync(
		UpdateBrandAgentDividendTimeIntervalCommand command,
		CancellationToken cancellationToken = default)
		=> tgsRepository.UpdateBrandAgentDividendTimeIntervalAsync(
			command.BrandId,
			command.PreviousSettlementFrom,
			command.PreviousSettlementTo,
			command.CurrentSettlementFrom,
			command.CurrentSettlementTo,
			command.NextSettlementFrom,
			command.NextSettlementTo,
			cancellationToken);
}
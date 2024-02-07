using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandAgentDividendTimeIntervalCommandService : ICommandService<UpdateBrandAgentDividendTimeIntervalCommand>
{
	private readonly ITgsRepository _tgsRepository;

	public UpdateBrandAgentDividendTimeIntervalCommandService(
		ITgsRepository tgsRepository)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
	}

	public ValueTask ExecuteAsync(
		UpdateBrandAgentDividendTimeIntervalCommand command, 
		CancellationToken cancellationToken = default) 
		=> _tgsRepository.UpdateBrandAgentDividendTimeIntervalAsync(
			command.BrandId, 
			command.PreviousSettlementFrom,
			command.PreviousSettlementTo,
			command.CurrentSettlementFrom,
			command.CurrentSettlementTo,
			command.NextSettlementFrom,
			command.NextSettlementTo,
			cancellationToken);
}
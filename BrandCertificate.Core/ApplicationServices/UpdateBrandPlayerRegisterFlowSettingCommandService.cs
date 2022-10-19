using Google.Protobuf;
using Tgs.BrandCertificateService.Protos;
using Tgs.MessageQueue;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class UpdateBrandPlayerRegisterFlowSettingCommandService : ICommandService<UpdateBrandPlayerRegisterFlowSettingCommand>
{
	private readonly ITgsRepository _tgsRepository;
	private readonly IMessageSender _sender;

	public UpdateBrandPlayerRegisterFlowSettingCommandService(
		ITgsRepository tgsRepository,
		IMessageSender sender)
	{
		_tgsRepository = tgsRepository ?? throw new ArgumentNullException(nameof(tgsRepository));
		_sender = sender ?? throw new ArgumentNullException(nameof(sender));
	}

	public async ValueTask ExecuteAsync(UpdateBrandPlayerRegisterFlowSettingCommand command, CancellationToken cancellationToken = default)
	{
		var isUpdateSuccess = await _tgsRepository
			.UpdateBrandPlayerRegisterFlowSettingAsync(command.BrandId, command.PlayerRegisterFlowSettings, cancellationToken)
			.ConfigureAwait(false);

		if (!isUpdateSuccess)
			return;

		var request = new BrandFrontendInfoChangedEvent
		{
			BrandId = command.BrandId
		};

		await _sender
			.PublishAsync("tgs.brand.frontendinfo.changed", request.ToByteArray(), cancellationToken)
			.ConfigureAwait(false);
	}
}
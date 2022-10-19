using Google.Protobuf;
using Tgs.BrandCertificateService.Protos;
using Tgs.MessageQueue;
using Tgs.ServiceUtility;

namespace Tgs.BrandCertificate.ApplicationServices;

internal class RemoveBrandFrontendInfoCachedCommandService : ICommandService<RemoveBrandFrontendInfoCachedCommand>
{
	private readonly IMessageSender _sender;

	public RemoveBrandFrontendInfoCachedCommandService(IMessageSender sender)
	{
		_sender = sender ?? throw new ArgumentNullException(nameof(sender));
	}

	public async ValueTask ExecuteAsync(RemoveBrandFrontendInfoCachedCommand command, CancellationToken cancellationToken = default)
	{
		var request = new BrandFrontendInfoChangedEvent
		{
			BrandId = command.BrandId
		};

		await _sender
			.PublishAsync("tgs.brand.frontendinfo.changed", request.ToByteArray(), cancellationToken)
			.ConfigureAwait(false);
	}
}
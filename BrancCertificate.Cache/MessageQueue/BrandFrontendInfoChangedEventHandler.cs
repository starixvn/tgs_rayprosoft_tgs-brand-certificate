using Tgs.BrandCertificateService.Protos;
using Valhalla.MessageQueue;

namespace Tgs.BrandCertificate.Cache.MessageQueue;

internal class BrandFrontendInfoChangedEventHandler(IBrandFrontendInfoCacheCleaner brandFrontendInfoCacheCleaner)
	: IMessageHandler<ReadOnlyMemory<byte>>
{
	private readonly IBrandFrontendInfoCacheCleaner _brandFrontendInfoCacheCleaner = brandFrontendInfoCacheCleaner ?? throw new ArgumentNullException(nameof(brandFrontendInfoCacheCleaner));

	public ValueTask HandleAsync(
		string subject,
		ReadOnlyMemory<byte> data,
		IEnumerable<MessageHeaderValue>? headerValues,
		CancellationToken cancellationToken = default)
	{
		var eventData = BrandFrontendInfoChangedEvent.Parser.ParseFrom(data.Span);

		_brandFrontendInfoCacheCleaner.Clean(eventData.BrandId);

		return ValueTask.CompletedTask;
	}
}

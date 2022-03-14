using MailContainerTest.Data;
using MailContainerTest.Exceptions;
using MailContainerTest.Extensions;
using MailContainerTest.Types;
using Microsoft.Extensions.Options;

namespace MailContainerTest.Services;

/// <inheritdoc />
public class MailTransferService : IMailTransferService
{
    private readonly IMailContainerDataStoreFactory _mailDataStoreFactory;
    private readonly IOptions<MailContainerSettings> _options;

    /// <summary>
    /// Creates an instance with the supplied parameters.
    /// </summary>
    /// <param name="mailDataStoreFactory">The <see cref="IMailContainerDataStoreFactory"/> with which to access underlying data stores.</param>
    /// <param name="options">The <see cref="IOptions{MailContainerSettings}"/> with which to configure the service.</param>
    public MailTransferService(IMailContainerDataStoreFactory mailDataStoreFactory, IOptions<MailContainerSettings> options)
    {
        _mailDataStoreFactory = mailDataStoreFactory;
        _options = options;
    }

    /// <inheritdoc />
    /// <exception cref="InvalidTransferRequestException">When the request is invalid.</exception>
    public MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request)
    {
        ValidateRequest(request);

        IMailContainerDataStore dataStore = _mailDataStoreFactory.Create(_options.Value.DataStoreType);

        MailContainer sourceContainer = dataStore.GetMailContainer(request.SourceMailContainerNumber);
        MailContainer destinationContainer = dataStore.GetMailContainer(request.DestinationMailContainerNumber);

        MakeMailTransferResult result = new() { FailureReason = request.Validate(sourceContainer, destinationContainer) };

        if (!result.Success)
        {
            return result;
        }

        sourceContainer.TotalItems -= request.NumberOfMailItems;
        destinationContainer.TotalItems += request.NumberOfMailItems;

        dataStore.UpdateMailContainer(sourceContainer);
        dataStore.UpdateMailContainer(destinationContainer);

        return result;
    }

    private static void ValidateRequest(MakeMailTransferRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.SourceMailContainerNumber)
            || string.IsNullOrWhiteSpace(request.DestinationMailContainerNumber)
            || request.NumberOfMailItems < 1)
        {
            throw new InvalidTransferRequestException(request);
        }
    }
}
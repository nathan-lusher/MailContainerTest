using MailContainerTest.Types;

namespace MailContainerTest.Extensions;

internal static class MakeMailTransferRequestExtensions
{
    public static TransferFailureReason? Validate(this MakeMailTransferRequest request,
        MailContainer sourceContainer, MailContainer destinationContainer)
    {
        if (!sourceContainer.IsAllowed(request.MailType) || !destinationContainer.IsAllowed(request.MailType))
        {
            return TransferFailureReason.MailTypeNotAllowed;
        }

        if (sourceContainer.IsOutOfService() || !destinationContainer.IsOperational())
        {
            return TransferFailureReason.ContainerIsNotOperational;
        }

        if (!sourceContainer.HasItems(request.NumberOfMailItems))
        {
            return TransferFailureReason.NotEnoughMailItems;
        }

        return null;
    }
}
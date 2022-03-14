using MailContainerTest.Types;

namespace MailContainerTest.Services;

/// <summary>
/// Service for transferring mail items between containers.
/// </summary>
public interface IMailTransferService
{
    /// <summary>
    /// Transfers a mail items from one container to another, based on the supplied request.
    /// </summary>
    /// <param name="request">The transfer request.</param>
    /// <returns>The result of the mail transfer action.</returns>
    MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request);
}
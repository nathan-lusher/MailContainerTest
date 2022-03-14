namespace MailContainerTest.Types;

/// <summary>
/// A request to transfer mail to a container.
/// </summary>
public record MakeMailTransferRequest
{
    /// <summary>
    /// The number of the container to transfer.
    /// </summary>
    public string SourceMailContainerNumber { get; init; } = string.Empty;

    /// <summary>
    /// The number of the container into which the transfer should be made.
    /// </summary>
    public string DestinationMailContainerNumber { get; init; } = string.Empty;

    /// <summary>
    /// The number of mail items in the transfer.
    /// </summary>
    public int NumberOfMailItems { get; init; }

    /// <summary>
    /// The date and time to perform the transfer.
    /// </summary>
    /// <remarks>Not currently used.</remarks>
    public DateTime TransferDate { get; init; }

    /// <summary>
    /// The type of mail being transferred.
    /// </summary>
    public MailType MailType { get; init; }
}
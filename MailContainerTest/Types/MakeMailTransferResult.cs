namespace MailContainerTest.Types;

/// <summary>
/// The result of a mail transfer.
/// </summary>
public record MakeMailTransferResult
{
    /// <summary>
    /// Flag indicating whether the transfer was a success (see failure reason if success is false).
    /// </summary>
    public bool Success => FailureReason == null;

    /// <summary>
    /// The reason (if any) that the mail transfer failed.
    /// </summary>
    public TransferFailureReason? FailureReason { get; init; }
}
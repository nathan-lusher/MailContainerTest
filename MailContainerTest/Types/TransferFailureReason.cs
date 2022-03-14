namespace MailContainerTest.Types;

/// <summary>
/// The reason for a mail transfer failure.
/// </summary>
public enum TransferFailureReason
{
    /// <summary>
    /// The mail container does not have available capacity.
    /// </summary>
    NotEnoughMailItems,

    /// <summary>
    /// The container is not operational.
    /// </summary>
    ContainerIsNotOperational,

    /// <summary>
    /// The mail type is not supported by the container.
    /// </summary>
    MailTypeNotAllowed
}
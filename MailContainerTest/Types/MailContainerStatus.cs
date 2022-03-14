namespace MailContainerTest.Types;

/// <summary>
/// Status of the mail container.
/// </summary>
public enum MailContainerStatus
{
    /// <summary>
    /// Mail container is operational.
    /// </summary>
    Operational,

    /// <summary>
    /// Mail container is out of service.
    /// </summary>
    OutOfService,

    /// <summary>
    /// Mail container does not allow inward transfers.
    /// </summary>
    NoTransfersIn
}
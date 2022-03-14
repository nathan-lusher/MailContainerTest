namespace MailContainerTest.Types;

/// <summary>
/// A mail container.
/// </summary>
public record MailContainer
{
    /// <summary>
    /// The type of data store associated with this container.
    /// </summary>
    public MailContainerDataStoreType ContainerDataStoreType { get; set; }

    /// <summary>
    /// The mail container number.
    /// </summary>
    public string? MailContainerNumber { get; set; }

    /// <summary>
    /// The total number of items in the mail container.
    /// </summary>
    public int TotalItems { get; set; }

    /// <summary>
    /// The status of the mail container.
    /// </summary>
    public MailContainerStatus Status { get; set; }

    /// <summary>
    /// The type of mail that can be stored in the container.
    /// </summary>
    public MailType AllowedMailType { get; set; }
}
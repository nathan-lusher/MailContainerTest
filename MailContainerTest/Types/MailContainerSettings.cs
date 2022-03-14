namespace MailContainerTest.Types;

/// <summary>
/// Mail container settings.
/// </summary>
public record MailContainerSettings
{
    /// <summary>
    /// The type of data store for mail containers.
    /// </summary>
    public MailContainerDataStoreType DataStoreType { get; set; } = MailContainerDataStoreType.Standard;
}
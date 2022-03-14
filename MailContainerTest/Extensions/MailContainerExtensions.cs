using MailContainerTest.Types;

namespace MailContainerTest.Extensions;

internal static class MailContainerExtensions
{
    public static bool IsAllowed(this MailContainer? container, MailType mailType)
        => container?.AllowedMailType == mailType;

    public static bool IsOperational(this MailContainer? container)
        => container?.Status == MailContainerStatus.Operational;

    public static bool IsOutOfService(this MailContainer? container)
        => container?.Status == MailContainerStatus.OutOfService;

    public static bool HasItems(this MailContainer? container, int mailItemCount)
        => container?.TotalItems >= mailItemCount;
}
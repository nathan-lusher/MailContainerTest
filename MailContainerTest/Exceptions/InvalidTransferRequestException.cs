using MailContainerTest.Types;

namespace MailContainerTest.Exceptions;

/// <summary>
/// Exception raised when an invalid transfer request is made.
/// </summary>
public class InvalidTransferRequestException : Exception
{
    /// <summary>
    /// The invalid request that caused the exception.
    /// </summary>
    public MakeMailTransferRequest Request { get; set; }

    /// <summary>
    /// Initialises the instance with the supplied parameter.
    /// </summary>
    /// <param name="request">The invalid request.</param>
    public InvalidTransferRequestException(MakeMailTransferRequest request)
    {
        Request = request;
    }
}
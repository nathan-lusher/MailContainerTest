using MailContainerTest.Data;
using MailContainerTest.Exceptions;
using MailContainerTest.Services;
using MailContainerTest.Types;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

#pragma warning disable CS8601

namespace MailContainerTest.Tests;

public class MailTransferServiceTests
{
    private readonly Mock<IMailContainerDataStoreFactory> _mockDataStoreFactory;
    private readonly Mock<IMailContainerDataStore> _mockDataStore;
    private readonly Mock<IOptions<MailContainerSettings>> _mockOptions;
    private readonly string _sourceContainerNumber = Guid.NewGuid().ToString();
    private readonly string _destinationContainerNumber = Guid.NewGuid().ToString();

    private MailContainerSettings _mailContainerSettings;

    public MailTransferServiceTests()
    {
        _mockDataStore = new Mock<IMailContainerDataStore>();
        _mockDataStoreFactory = new Mock<IMailContainerDataStoreFactory>();

        _mockDataStoreFactory
            .Setup(f => f.Create(It.IsAny<MailContainerDataStoreType>()))
            .Returns(() => _mockDataStore.Object);

        _mailContainerSettings = new MailContainerSettings();

        _mockOptions = new Mock<IOptions<MailContainerSettings>>();
        _mockOptions
            .Setup(o => o.Value)
            .Returns(() => _mailContainerSettings);
    }

    [Theory]
    [InlineData(MailType.StandardLetter)]
    [InlineData(MailType.LargeLetter)]
    [InlineData(MailType.SmallParcel)]
    public void CanPerformMailTransferForCorrectType(MailType allowedMailType)
    {
        MailContainer sourceContainer = new() { AllowedMailType = allowedMailType, TotalItems = 400 };
        MailContainer destinationContainer = new() { AllowedMailType = allowedMailType };

        ConfigureMockDataStore(sourceContainer, destinationContainer);
        MakeMailTransferRequest request = CreateRequest(allowedMailType);

        MakeMailTransferResult result = MakeMailTransfer(request);

        Assert.True(result.Success);
        Assert.Null(result.FailureReason);
    }

    [Theory]
    [InlineData(MailType.StandardLetter, MailType.LargeLetter)]
    [InlineData(MailType.LargeLetter, MailType.SmallParcel)]
    [InlineData(MailType.SmallParcel, MailType.StandardLetter)]
    public void CannotPerformMailTransferForIncorrectType(MailType allowedMailType, MailType requestMailType)
    {
        MailContainer sourceContainer = new() { AllowedMailType = allowedMailType, TotalItems = 400 };
        MailContainer destinationContainer = new() { AllowedMailType = allowedMailType };

        ConfigureMockDataStore(sourceContainer, destinationContainer);
        MakeMailTransferRequest request = CreateRequest(requestMailType);

        MakeMailTransferResult result = MakeMailTransfer(request);

        Assert.False(result.Success);
        Assert.Equal(TransferFailureReason.MailTypeNotAllowed, result.FailureReason);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ExceptionIsThrownWhenSourceContainerNumberIsInvalid(string? containerNumber)
    {
        MakeMailTransferRequest request = new() { SourceMailContainerNumber = containerNumber };

        var exception = Assert.Throws<InvalidTransferRequestException>(() => MakeMailTransfer(request));
        Assert.Equal(exception.Request, request);

    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void ExceptionIsThrownWhenDestinationContainerNumberIsInvalid(string? containerNumber)
    {
        MakeMailTransferRequest request = new() { DestinationMailContainerNumber = containerNumber };

        var exception = Assert.Throws<InvalidTransferRequestException>(() => MakeMailTransfer(request));
        Assert.Equal(exception.Request, request);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void ExceptionIsThrownWhenNumberOfMailItemsIsInvalid(int numberOfMailItems)
    {
        MakeMailTransferRequest request = new()
        {
            SourceMailContainerNumber = "1",
            DestinationMailContainerNumber = "2",
            NumberOfMailItems = numberOfMailItems
        };

        var exception = Assert.Throws<InvalidTransferRequestException>(() => MakeMailTransfer(request));
        Assert.Equal(exception.Request, request);
    }

    [Fact]
    public void CannotPerformTransferOfMoreThanTotalItemsInContainer()
    {
        ConfigureMockDataStore(new MailContainer { TotalItems = 4 }, new MailContainer());
        MakeMailTransferRequest request = CreateRequest(numberOfMailItems: 8);

        MakeMailTransferResult result = MakeMailTransfer(request);

        Assert.False(result.Success);
        Assert.Equal(TransferFailureReason.NotEnoughMailItems, result.FailureReason);
    }

    [Theory]
    [InlineData(MailContainerStatus.NoTransfersIn, true)]
    [InlineData(MailContainerStatus.OutOfService, false)]
    [InlineData(MailContainerStatus.Operational, true)]
    public void CannotPerformTransferIfSourceContainerIsNotInCorrectState(MailContainerStatus mailContainerStatus, bool expectedSuccess)
    {
        MailContainer sourceContainer = new() { Status = mailContainerStatus, TotalItems = 400 };
        MailContainer destinationContainer = new() { Status = MailContainerStatus.Operational };

        ConfigureMockDataStore(sourceContainer, destinationContainer);

        MakeMailTransferRequest request = CreateRequest();

        MakeMailTransferResult result = MakeMailTransfer(request);

        Assert.Equal(expectedSuccess, result.Success);
        Assert.Equal(expectedSuccess ? null : TransferFailureReason.ContainerIsNotOperational, result.FailureReason);
    }

    [Theory]
    [InlineData(MailContainerStatus.NoTransfersIn, false)]
    [InlineData(MailContainerStatus.OutOfService, false)]
    [InlineData(MailContainerStatus.Operational, true)]
    public void CannotPerformTransferIfDestinationContainerIsNotInCorrectState(MailContainerStatus mailContainerStatus, bool expectedSuccess)
    {
        MailContainer sourceContainer = new() { Status = MailContainerStatus.Operational, TotalItems = 400 };
        MailContainer destinationContainer = new() { Status = mailContainerStatus };

        ConfigureMockDataStore(sourceContainer, destinationContainer);

        MakeMailTransferRequest request = CreateRequest();

        MakeMailTransferResult result = MakeMailTransfer(request);

        Assert.Equal(expectedSuccess, result.Success);
        Assert.Equal(expectedSuccess ? null : TransferFailureReason.ContainerIsNotOperational, result.FailureReason);
    }

    [Fact]
    public void CapacityIsUpdatedAfterTransfer()
    {
        var sourceContainer = new MailContainer { TotalItems = 100 };
        var destinationContainer = new MailContainer { TotalItems = 25 };

        ConfigureMockDataStore(sourceContainer, destinationContainer);
        MakeMailTransferRequest request = CreateRequest(numberOfMailItems: 75);

        MakeMailTransferResult result = MakeMailTransfer(request);

        Assert.True(result.Success);
        Assert.Equal(25, sourceContainer.TotalItems);
        Assert.Equal(100, destinationContainer.TotalItems);
    }

    [Fact]
    public void DataStoreIsUpdatedAfterTransfer()
    {
        var sourceContainer = new MailContainer
        {
            MailContainerNumber = _sourceContainerNumber,
            TotalItems = 100
        };

        var destinationContainer = new MailContainer
        {
            MailContainerNumber = _destinationContainerNumber,
            TotalItems = 25
        };

        _mockDataStore
            .Setup(s => s.GetMailContainer(_sourceContainerNumber))
            .Returns(sourceContainer);

        _mockDataStore
            .Setup(s => s.GetMailContainer(_destinationContainerNumber))
            .Returns(destinationContainer);

        MakeMailTransferRequest request = CreateRequest();

        MakeMailTransferResult result = MakeMailTransfer(request);

        Assert.True(result.Success);

        _mockDataStore.Verify(s => s.UpdateMailContainer(sourceContainer), Times.Once);
        _mockDataStore.Verify(s => s.UpdateMailContainer(destinationContainer), Times.Once);
    }

    [Theory]
    [InlineData(MailContainerDataStoreType.Standard)]
    [InlineData(MailContainerDataStoreType.Backup)]
    public void DataStoreIsCreatedWithCorrectType(MailContainerDataStoreType dataStoreType)
    {
        _mailContainerSettings = new MailContainerSettings { DataStoreType = dataStoreType };

        CreateService().MakeMailTransfer(CreateRequest());

        _mockDataStoreFactory.Verify(f => f.Create(dataStoreType), Times.Once);
    }

    private void ConfigureMockDataStore(MailContainer sourceContainer, MailContainer destinationContainer)
    {
        _mockDataStore
            .Setup(d => d.GetMailContainer(It.Is<string>(
                s => s == _sourceContainerNumber)))
            .Returns(sourceContainer);

        _mockDataStore
            .Setup(d => d.GetMailContainer(It.Is<string>(
                s => s == _destinationContainerNumber)))
            .Returns(destinationContainer);
    }

    private MakeMailTransferRequest CreateRequest(MailType allowedMailType = MailType.StandardLetter, int numberOfMailItems = 10)
    {
        return new MakeMailTransferRequest
        {
            SourceMailContainerNumber = _sourceContainerNumber,
            DestinationMailContainerNumber = _destinationContainerNumber,
            MailType = allowedMailType,
            NumberOfMailItems = numberOfMailItems,
            TransferDate = DateTime.Now
        };
    }

    private MakeMailTransferResult MakeMailTransfer(MakeMailTransferRequest request)
    {
        return CreateService().MakeMailTransfer(request);
    }

    private IMailTransferService CreateService()
        => new MailTransferService(_mockDataStoreFactory.Object, _mockOptions.Object);
}
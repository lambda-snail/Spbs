using System.Net;
using AutoFixture;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Spbs.Data.Cosmos;
using Spbs.Shared.Data;
using Spbs.Ui.Data.Messaging.Commands;
using Spbs.Ui.Features.RecurringExpenses;
using Spbs.Ui.Features.RecurringExpenses.Mapping;
using Spbs.Ui.Features.RecurringExpenses.Messaging;

namespace Spbs.RecurringExpenses.Tests;

public class RecurringExpenseWriterTests
{
    private Fixture _fixture;
    
    private Mock<IDateTimeProvider> _dateTimeMock;
    private Mock<CosmosClient> _clientMock;
    private Mock<Database> _databaseMock;
    private Mock<Container> _containerMock;
    private Mock<ILogger<RecurringExpenseWriter>> _loggerMock;

    private Mock<RecurringExpenses_CreateExpenseCommandPublisher> _publisherMock;
    
    private IMapper _mapper;

    private DataConfigurationOptions _dco;
    private Mock<IOptions<DataConfigurationOptions>> _optionsMock;

    private static readonly string _cosmosType = CosmosTypeConstants.SpbsRecurringExpenses;
    
    public RecurringExpenseWriterTests()
    {
        _fixture = new();
        
        _dateTimeMock = new();
        _clientMock = new();
        _databaseMock = new();
        _containerMock = new();

        _databaseMock
            .Setup(db => db.GetContainer(It.IsAny<string>()))
            .Returns(_containerMock.Object);

        _clientMock
            .Setup(c => c.GetDatabase(It.IsAny<string>()))
            .Returns(_databaseMock.Object);
        
        _publisherMock = new();

        var c = new MapperConfiguration(
            cfg => cfg.AddProfile(new RecurringExpenseMapperProfile())
        );
        _mapper = c.CreateMapper();

        _dco = _fixture.Create<DataConfigurationOptions>();
        _optionsMock = new Mock<IOptions<DataConfigurationOptions>>();
        _optionsMock.Setup(o => o.Value).Returns(_dco);

        _loggerMock = new();
    }

    private RecurringExpenseWriter GetInstance()
    {
        return new(
            _clientMock.Object,
            _publisherMock.Object,
            _mapper,
            _dateTimeMock.Object,
            _optionsMock.Object,
            _loggerMock.Object
        );
    }
    
    [Fact(DisplayName = "Happy case when upserting recurring expenses")]
    public async Task UpsertExpenseAsync_ExpenseWithId_ShouldCallUpsertItemAsync()
    {
        // Arrange
        var e = _fixture.Create<RecurringExpense>();
        
        var itemResponseMock = CreateItemResponseMock(e, HttpStatusCode.OK);
        SetupContainerMock(itemResponseMock);
        
        var expenseWriter = GetInstance();
        
        // Act
        var upserted = await expenseWriter.UpsertExpenseAsync(e);
        
        // Assert
        Assert.NotNull(upserted);
        _containerMock.Verify(c => 
            c.UpsertItemAsync(
                It.Is<CosmosDocument<RecurringExpense>>(
                    cd => 
                        cd.Id == e.Id &&
                        cd.Type == _cosmosType &&
                        cd.Data.UserId == e.UserId
                ), 
                null, 
                null,
            default   
            ), 
            Times.Once);
    }

    [Fact(DisplayName = "Failed upserts return null without crashing")]
    public async Task UpsertExpenseAsync_ExpenseWithIdFailedUpsert_ShouldReturnNull()
    {
        // Arrange
        var e = _fixture.Create<RecurringExpense>();
        
        var itemResponseMock = CreateItemResponseMock(null, HttpStatusCode.InternalServerError);
        SetupContainerMock(itemResponseMock);
        
        var expenseWriter = GetInstance();
        
        // Act
        var upserted = await expenseWriter.UpsertExpenseAsync(e);

        // Assert
        Assert.Null(upserted);
    }
    
    [Fact(DisplayName = "Happy case when creating recurring expenses")]
    public async Task UpsertExpenseAsync_ExpenseWithoutId_ShouldReturnWithId()
    {
        // Arrange
        var e = _fixture.Build<RecurringExpense>().Without(e => e.Id).Create();
        
        var itemResponseMock = CreateItemResponseMock(e, HttpStatusCode.OK);
        SetupContainerMock(itemResponseMock);
        
        var expenseWriter = GetInstance();
        
        // Act
        var upserted = await expenseWriter.UpsertExpenseAsync(e);

        // Assert
        Assert.NotEqual(Guid.Empty, e.Id);
    }

    [Fact(DisplayName = "Creating a recurring expense should publish a message to the queue")]
    public async Task UpsertExpenseAsync_ExpenseWithoutId_ShouldPublishMessage()
    {
        // Arrange
        var e = _fixture.Build<RecurringExpense>().Without(e => e.Id).Create();
        
        var itemResponseMock = CreateItemResponseMock(e, HttpStatusCode.OK);
        SetupContainerMock(itemResponseMock);

        DateTime today = new DateTime(2023, 07, 04);
        _dateTimeMock.Setup(dt => dt.Now()).Returns(today);
        
        var expenseWriter = GetInstance();
        
        // Act
        var upserted = await expenseWriter.UpsertExpenseAsync(e);

        // Assert
        _publisherMock.Verify(p => p.ScheduleMessage(
        
            It.Is<CreateExpenseCommand>(cmd => 
                cmd.Expense.RecurringExpenseId == e.Id &&
                cmd.Expense.UserId == e.UserId),
            It.Is<DateTime>(dt => dt == e.GetNextBillingDate(today))
        ));
    }
    
    private void SetupContainerMock(Mock<ItemResponse<CosmosDocument<RecurringExpense>>> itemResponseMock)
    {
        _containerMock
            .Setup(c => c.UpsertItemAsync(It.IsAny<CosmosDocument<RecurringExpense>>(), null, null, default))
            .Returns(Task.FromResult(itemResponseMock.Object));
    }

    private Mock<ItemResponse<CosmosDocument<RecurringExpense>>> CreateItemResponseMock(RecurringExpense? e, HttpStatusCode status)
    {
        var itemResponseMock = new Mock<ItemResponse<CosmosDocument<RecurringExpense>>>();
        itemResponseMock
            .Setup(r => r.StatusCode)
            .Returns(status);

        if (e is not null)
        {
            itemResponseMock
                .Setup(r => r.Resource)
                .Returns(new CosmosDocument<RecurringExpense>()
                {
                    Id = e.Id,
                    Data = e,
                    Type = _cosmosType
                });
        }

        return itemResponseMock;
    }
}
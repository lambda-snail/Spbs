using AutoMapper;
using Spbs.Ui.Features.RecurringExpenses.Mapping;

namespace Spbs.RecurringExpenses.Tests;

public class RecurringExpenseMapperProfileTests
{
    private readonly MapperConfiguration _config;
    private readonly IMapper _mapper;

    public RecurringExpenseMapperProfileTests()
    {
        _config = new MapperConfiguration(cfg =>
            cfg.AddProfile(new RecurringExpenseMapperProfile())
        );

        _mapper = _config.CreateMapper();
    }

    [Fact(DisplayName = "Assert that the configuration is valid")]
    public void MapperConfiguration_IsValid()
    {
        _config.AssertConfigurationIsValid();
    }
}